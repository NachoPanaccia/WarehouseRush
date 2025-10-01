using UnityEngine;

public class CamionManager : MonoBehaviour
{
    public static CamionManager Instance { get; private set; }

    [Header("Spawns y prefabs")]
    public Transform[] puntosInicio;
    public GameObject[] prefabsDeCamiones;
    public int cantidadDeCamiones = 3;

    private PilaDeCamionesDinamica pila;

    private void Awake() => Instance = this;

    void Start()
    {
        if (puntosInicio == null || puntosInicio.Length == 0)
        {
            Debug.LogError("No se asignaron puntos de inicio para los camiones.");
            return;
        }
        if (prefabsDeCamiones == null || prefabsDeCamiones.Length == 0)
        {
            Debug.LogError("No se asignaron prefabs de camiones.");
            return;
        }

        pila = new PilaDeCamionesDinamica();
        pila.InicializarPila();

        for (int i = 0; i < cantidadDeCamiones; i++)
        {
            var punto = puntosInicio[i % puntosInicio.Length];
            var prefab = prefabsDeCamiones[Random.Range(0, prefabsDeCamiones.Length)];
            var camion = Instantiate(prefab, punto.position, punto.rotation);

            // luz inicialmente off
            var luz = camion.GetComponentInChildren<Light>();
            if (luz) luz.enabled = false;

            pila.Apilar(camion);
        }

        ActivarLuzDelTope();
    }

    public void EliminarCamion(GameObject camion)
    {
        if (pila.Peek() == camion)
        {
            DespacharCamion();
        }
        else
        {
            Debug.LogWarning("El camión no es el último ingresado (LIFO). No se elimina.");
        }
    }

    private void DespacharCamion()
    {
        var camion = pila.Desapilar();
        if (camion)
        {
            var luz = camion.GetComponentInChildren<Light>();
            if (luz) luz.enabled = false;
            Destroy(camion);
        }
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
}
