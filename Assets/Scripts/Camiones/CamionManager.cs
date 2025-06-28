using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamionManager : MonoBehaviour
{
    public static CamionManager Instance { get; private set; }

    public Transform[] puntosInicio;
    public GameObject[] prefabsDeCamiones;
    public int cantidadDeCamiones = 3;

    private PilaDeCamionesDinamica pilaCamiones;

    void Start()
    {
        Instance = this;

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

        pilaCamiones = new PilaDeCamionesDinamica();
        pilaCamiones.InicializarPila();

        for (int i = 0; i < cantidadDeCamiones; i++)
        {
            Transform punto = puntosInicio[i % puntosInicio.Length];

            GameObject nuevoCamion = Instantiate(
                prefabsDeCamiones[Random.Range(0, prefabsDeCamiones.Length)],
                punto.position,
                Quaternion.identity);

            // Asegurarse que la luz esté inicialmente desactivada
            Light luz = nuevoCamion.GetComponentInChildren<Light>();
            if (luz != null) luz.enabled = false;

            pilaCamiones.Apilar(nuevoCamion);
        }

        ActivarLuzCamionActual();
    }

    public void DespacharCamion()
    {
        GameObject camionDespachado = pilaCamiones.Desapilar();

        if (camionDespachado != null)
        {
            Destroy(camionDespachado);
        }

        if (pilaCamiones.PilaVacia())
        {
            Debug.Log("Todos los camiones fueron despachados.");
        }
        else
        {
            ActivarLuzCamionActual();
        }
    }

    public void EliminarCamion(GameObject camion)
    {
        if (pilaCamiones.Tope() == camion)
        {
            DespacharCamion();
        }
        else
        {
            Debug.LogWarning("El camión no es el último ingresado. No se puede eliminar por LIFO.");
        }
    }

    public GameObject ObtenerCamionActual()
    {
        return pilaCamiones.Tope();
    }

    public int CantidadCamionesActivos()
    {
        int contador = 0;
        NodoPilaCamion actual = GetNodoTope();
        while (actual != null)
        {
            contador++;
            actual = actual.siguiente;
        }
        return contador;
    }

    private NodoPilaCamion GetNodoTope()
    {
        System.Reflection.FieldInfo field = typeof(PilaDeCamionesDinamica).GetField("tope", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (NodoPilaCamion)field.GetValue(pilaCamiones);
    }

    private void ActivarLuzCamionActual()
    {
        NodoPilaCamion actual = GetNodoTope();

        while (actual != null)
        {
            Light luz = actual.dato.GetComponentInChildren<Light>();
            if (luz != null)
            {
                luz.enabled = (actual == GetNodoTope());
            }
            actual = actual.siguiente;
        }
    }
}
