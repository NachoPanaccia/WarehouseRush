using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CamionManager : MonoBehaviour
{
    public static CamionManager Instance { get; private set; }

    [Header("Descubrimiento")]
    [Tooltip("Opcional: si lo asignás, solo busca camiones dentro de este transform.")]
    [SerializeField] private Transform raizCamiones;

    [Tooltip("Si todos los camiones tienen CamionOrden, se usa ese valor (ascendente).")]
    [SerializeField] private bool usarOrdenPersonalizado = true;

    [Header("Indicadores de orden (visual)")]
    [SerializeField] private bool mostrarOrdenSalida = true;
    [SerializeField] private Vector3 labelOffset = new Vector3(0f, 2.0f, 0f);
    [SerializeField] private int labelFontSize = 3;                // TextMeshPro 3D
    [SerializeField] private Color labelColor = Color.white;

    private PilaDeCamionesDinamica pila;
    // Lista donde el índice 0 es el PRÓXIMO EN SALIR (tope de la pila)
    private readonly List<GameObject> ordenTopPrimero = new();

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
            Debug.LogWarning("No se encontraron camiones en la escena.");
            return;
        }

        // 2) Orden base
        var lista = camiones.ToList();

        if (usarOrdenPersonalizado && lista.All(c => c.TryGetComponent<CamionOrden>(out _)))
        {
            lista = lista.OrderBy(c => c.GetComponent<CamionOrden>().orden).ToList();
        }
        else if (raizCamiones != null)
        {
            lista = lista.OrderBy(c => c.transform.GetSiblingIndex()).ToList();
        }

        // 3) Armar pila (LIFO) y preparar labels (apagamos luces inicialmente)
        foreach (var c in lista)
        {
            var luz = c.GetComponentInChildren<Light>();
            if (luz) luz.enabled = false;

            pila.Apilar(c.gameObject);
        }

        // Construimos la vista "top primero" (reverse de lista)
        ordenTopPrimero.Clear();
        ordenTopPrimero.AddRange(lista.AsEnumerable().Reverse().Select(x => x.gameObject));

        if (mostrarOrdenSalida) ActualizarLabels();

        ActivarLuzDelTope();
    }

    // --- API desde CamionController ---
    public void NotificarCamionListo(CamionController camion)
    {
        if (pila.Peek() == camion.gameObject)
        {
            DespacharCamion();
        }
        else
        {
            Debug.LogWarning("Este camión no es el tope de la pila (LIFO). No se despacha.");
        }
    }

    // --- Helpers de despacho ---
    private void DespacharCamion()
    {
        var camion = pila.Desapilar();
        if (camion)
        {
            QuitarLabel(camion);
            var luz = camion.GetComponentInChildren<Light>();
            if (luz) luz.enabled = false;

            // Remover de la lista visual (top primero)
            ordenTopPrimero.Remove(camion);

            Destroy(camion);
        }

        if (mostrarOrdenSalida) ActualizarLabels();
        ActivarLuzDelTope();
    }

    private void ActivarLuzDelTope()
    {
        var top = pila.Peek();
        if (!top) return;
        var luz = top.GetComponentInChildren<Light>();
        if (luz) luz.enabled = true;
    }

    public GameObject ObtenerCamionActual() => pila.Peek();
    public int CantidadCamionesActivos() => pila.Count;

    // --- Descubrimiento compatible con versiones ---
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

    // --- Labels ---
    private void ActualizarLabels()
    {
        for (int i = 0; i < ordenTopPrimero.Count; i++)
        {
            var go = ordenTopPrimero[i];
            CrearOActualizarLabel(go, i + 1); // 1 = próximo en salir
        }
    }

    private void CrearOActualizarLabel(GameObject camion, int numero)
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
            tmp.text = numero.ToString();

            // Billboard para mirar a la cámara
            go.AddComponent<BillboardSimple>();
        }
        else
        {
            tmp = t.GetComponent<TextMeshPro>();
            if (tmp == null) tmp = t.gameObject.AddComponent<TextMeshPro>();
            tmp.text = numero.ToString();
            tmp.fontSize = labelFontSize;
            tmp.color = labelColor;
        }
    }

    private void QuitarLabel(GameObject camion)
    {
        var t = camion.transform.Find("__OrdenLabel");
        if (t != null) Destroy(t.gameObject);
    }
}
