using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CamionManager : MonoBehaviour
{
    public static CamionManager Instance { get; private set; }

    [Header("Descubrimiento")]
    [Tooltip("Si se asigna, busca camiones dentro de este transform; si no, busca en toda la escena.")]
    [SerializeField] private Transform raizCamiones;

    [Tooltip("Si true, empates de peso se resuelven por SiblingIndex (estable).")]
    [SerializeField] private bool usarTieBreaker = true;

    [Header("Indicadores sobre camión")]
    [SerializeField] private bool mostrarOrdenSalida = true;
    [SerializeField] private Vector3 labelOffset = new Vector3(0f, 2f, 0f);
    [SerializeField] private int labelFontSize = 3;
    [SerializeField] private Color labelColor = Color.white;

    [Header("Debug")]
    [SerializeField] private bool logOrdenEnConsola = true;
    [SerializeField] private bool mostrarPesoEnLabel = false;
    [SerializeField] private KeyCode togglePesoDebugKey = KeyCode.F3;

    private PilaDeCamionesDinamica pila;                 // stack físico (LIFO real)
    private readonly List<GameObject> ordenTopPrimero = new(); // lista lógica: índice 0 = próximo en salir (ASC por peso)

    private void Awake() => Instance = this;

    private void Start()
    {
        pila = new PilaDeCamionesDinamica();
        pila.InicializarPila();

        // 1) Encontrar todos los camiones
        CamionController[] camiones = raizCamiones
            ? raizCamiones.GetComponentsInChildren<CamionController>(true)
            : FindCamionesInScene();

        if (camiones == null || camiones.Length == 0)
        {
            Debug.LogWarning("[CamionManager] No se encontraron camiones en la escena.");
            return;
        }

        // 2) Armar datos (peso + tie-breaker)
        var datos = new List<(GameObject go, string name, int peso, int tie)>(camiones.Length);
        foreach (var c in camiones)
        {
            if (!c.TryGetComponent(out PesoDeCamion pesoComp))
            {
                // Si el prefab no lo tiene, lo agregamos; Awake se ejecuta y asigna peso [1..50]
                pesoComp = c.gameObject.AddComponent<PesoDeCamion>();
            }

            int tie = c.transform.GetSiblingIndex();
            string nombre = c.gameObject.name;
            datos.Add((c.gameObject, nombre, pesoComp.Peso, tie));
        }

        if (logOrdenEnConsola)
        {
            Debug.Log("[CamionManager] ORDEN ANTES de Quicksort:\n" + Formatear(datos));
        }

        // 3) ORDENAR con Quicksort por peso ASC (menor sale primero). Empates opcionales por tie-breaker.
        QuickSorter.QuickSort(datos, (a, b) =>
        {
            int cmp = a.peso.CompareTo(b.peso);
            if (cmp != 0) return cmp;
            if (!usarTieBreaker) return 0;
            return a.tie.CompareTo(b.tie);
        });

        if (logOrdenEnConsola)
        {
            Debug.Log("[CamionManager] ORDEN DESPUÉS de Quicksort (ASC por peso):\n" + Formatear(datos));
        }

        // 4) Construir la lista lógica "top primero" (ASC por peso)
        ordenTopPrimero.Clear();
        ordenTopPrimero.AddRange(datos.Select(d => d.go));

        // 5) Construir pila LIFO física: apilar en reversa para que el primero (ASC) quede arriba del stack
        //    - último en apilar = datos[0] => tope del stack es el de menor peso
        for (int i = datos.Count - 1; i >= 0; i--)
        {
            var go = datos[i].go;

            // apago todas las luces de arranque
            var l = go.GetComponentInChildren<Light>(true);
            if (l) l.enabled = false;

            pila.Apilar(go);
        }

        // 6) Labels y luz del tope (fija, sin parpadeo)
        if (mostrarOrdenSalida) ActualizarLabels(datos);
        EncenderLuzDelTope();
    }

    private void Update()
    {
        if (Input.GetKeyDown(togglePesoDebugKey))
        {
            mostrarPesoEnLabel = !mostrarPesoEnLabel;
            if (mostrarOrdenSalida) ActualizarLabels(null); // se rehace con los pesos actuales
        }
    }

    // === API desde CamionController ===
    public void NotificarCamionListo(CamionController camion)
    {
        if (pila.Peek() == camion.gameObject)
        {
            DespacharCamion();
        }
        else
        {
            Debug.LogWarning("[CamionManager] Este camión no es el tope de la pila (LIFO). No se despacha.");
        }
    }

    // === Despacho ===
    private void DespacharCamion()
    {
        var camion = pila.Desapilar();
        if (camion)
        {
            QuitarLabel(camion);

            // Apagar su luz
            var l = camion.GetComponentInChildren<Light>(true);
            if (l) l.enabled = false;

            ordenTopPrimero.Remove(camion);
            Destroy(camion);
        }

        if (mostrarOrdenSalida) ActualizarLabels(null);
        EncenderLuzDelTope();
    }

    private void EncenderLuzDelTope()
    {
        // Apagar todas primero
        foreach (var go in ordenTopPrimero)
        {
            if (!go) continue;
            var l = go.GetComponentInChildren<Light>(true);
            if (l) l.enabled = false;
        }

        // Encender solo la del tope real
        var top = pila.Peek();
        if (!top) return;
        var luzTop = top.GetComponentInChildren<Light>(true);
        if (luzTop) luzTop.enabled = true;
    }

    public GameObject ObtenerCamionActual() => pila.Peek();
    public int CantidadCamionesActivos() => pila.Count;

    // === Descubrimiento compatible ===
    private CamionController[] FindCamionesInScene()
    {
#if UNITY_2023_1_OR_NEWER
        return Object.FindObjectsByType<CamionController>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );
#else
        return Object.FindObjectsOfType<CamionController>(true);
#endif
    }

    // === Labels ===
    private void ActualizarLabels(List<(GameObject go, string name, int peso, int tie)> cacheOrdenAsc)
    {
        // Si no me pasaron datos, vuelvo a mirar pesos actuales para texto (por si togglearon debug).
        Dictionary<GameObject, int> pesos = null;
        if (mostrarPesoEnLabel)
        {
            pesos = new Dictionary<GameObject, int>(ordenTopPrimero.Count);
            foreach (var go in ordenTopPrimero)
            {
                if (!go) continue;
                if (go.TryGetComponent(out PesoDeCamion p))
                    pesos[go] = p.Peso;
            }
        }

        for (int i = 0; i < ordenTopPrimero.Count; i++)
        {
            var go = ordenTopPrimero[i];
            if (!go) continue;

            int numero = i + 1;

            string texto = numero.ToString();
            if (mostrarPesoEnLabel)
            {
                // Si tengo cache, uso el peso cacheado; sino, el que leí recién
                int peso;
                if (cacheOrdenAsc != null)
                {
                    var item = cacheOrdenAsc.FirstOrDefault(d => d.go == go);
                    peso = item.peso;
                }
                else
                {
                    peso = (pesos != null && pesos.TryGetValue(go, out var w)) ? w : -1;
                }
                if (peso >= 0) texto += $" ({peso})";
            }

            CrearOActualizarLabel(go, texto);
        }
    }

    private void CrearOActualizarLabel(GameObject camion, string texto)
    {
        const string labelName = "__OrdenLabel";
        Transform t = camion.transform.Find(labelName);
        TextMeshPro tmp;

        if (t == null)
        {
            var go = new GameObject(labelName);
            go.transform.SetParent(camion.transform, false);
            go.transform.localPosition = labelOffset;

            tmp = go.AddComponent<TextMeshPro>();
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = labelFontSize;
            tmp.color = labelColor;
            tmp.enableAutoSizing = false;
            tmp.text = texto;

            // Siempre mirando a la cámara
            go.AddComponent<BillboardSimple>();
        }
        else
        {
            tmp = t.GetComponent<TextMeshPro>() ?? t.gameObject.AddComponent<TextMeshPro>();
            tmp.text = texto;
            tmp.fontSize = labelFontSize;
            tmp.color = labelColor;
        }
    }

    private void QuitarLabel(GameObject camion)
    {
        var t = camion.transform.Find("__OrdenLabel");
        if (t != null) Destroy(t.gameObject);
    }

    private string Formatear(List<(GameObject go, string name, int peso, int tie)> datos)
    {
        // nombre(peso)[tie]
        return string.Join(" | ", datos.Select(d => $"{d.name}({d.peso})[{d.tie}]"));
    }
}
