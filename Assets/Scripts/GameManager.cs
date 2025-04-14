using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Configuración de camiones")]
    [SerializeField] private GameObject prefabCamion;
    [SerializeField] private Transform[] puntosSpawn;
    [SerializeField] private int cantidadCamionesEnNivel = 2;

    private PilaDeCamiones pilaCamiones = new PilaDeCamiones();

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
            }
        }
    }
}

