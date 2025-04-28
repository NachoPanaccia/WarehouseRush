using UnityEngine;
using System.Collections.Generic;

public class CamionManager : MonoBehaviour
{
    public static CamionManager Instance { get; private set; }

    [SerializeField] private GameObject prefabCamion;
    [SerializeField] private Transform[] puntosSpawn;
    [SerializeField] private int cantidadCamionesEnNivel = 2;

    private PilaDeCamiones pilaCamiones = new PilaDeCamiones();
    private List<GameObject> camionesActivos = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        CrearPilaDeCamiones(10);
        InstanciarCamionesDelNivel();
    }

    private void CrearPilaDeCamiones(int cantidadTotal)
    {
        for (int i = 0; i < cantidadTotal; i++)
        {
            GameObject nuevoCamion = prefabCamion;
            pilaCamiones.Apilar(nuevoCamion);
        }
    }

    private void InstanciarCamionesDelNivel()
    {
        for (int i = 0; i < cantidadCamionesEnNivel; i++)
        {
            if (!pilaCamiones.EstaVacia() && i < puntosSpawn.Length)
            {
                GameObject camionAInstanciar = pilaCamiones.Desapilar();
                GameObject instancia = Instantiate(camionAInstanciar, puntosSpawn[i].position, Quaternion.identity);
                instancia.name = "Camion_" + i;

                camionesActivos.Add(instancia); 
            }
        }
    }

    
    public void EliminarCamion(GameObject camion)
    {
        Debug.Log("🚚 Intentando eliminar camión: " + (camion != null ? camion.name : "null"));

        if (camionesActivos == null)
        {
            Debug.LogError("❌ camionesActivos es NULL en CamionManager!");
            return;
        }

        if (!camionesActivos.Contains(camion))
        {
            Debug.LogError("❌ camionesActivos NO contiene este camión: " + (camion != null ? camion.name : "null"));
            return;
        }

        camionesActivos.Remove(camion);
        Debug.Log("✅ Camión eliminado. Camiones restantes: " + camionesActivos.Count);

        if (camionesActivos.Count == 0)
        {
            Debug.Log("🏁 ¡Todos los camiones han sido procesados!");
            LevelManager.Instance.NivelCompleto();
        }
        if (camionesActivos.Count == 0)
        {
            Debug.Log("🏁 ¡Todos los camiones han sido procesados!");

            if (LevelManager.Instance == null)
            {
                Debug.LogError("❌ Error: LevelManager.Instance es NULL. No puedo pasar de nivel.");
            }
            else
            {
                Debug.Log("✅ LevelManager encontrado: " + LevelManager.Instance.gameObject.name);
                LevelManager.Instance.NivelCompleto();
            }
        }
    }
    public int CantidadCamionesActivos()
    {
        return camionesActivos.Count;
    }
}
