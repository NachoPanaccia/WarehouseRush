using UnityEngine;

public class ProbarRuta : MonoBehaviour
{
    public Transform jugador;
    public Transform objetivo;
    public GrafoDinamico grafo;
    public PathVisualizer visualizador;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            grafo.GenerarGrafo();

            var origen = grafo.BuscarMasCercano(jugador.position);
            var destino = grafo.BuscarMasCercano(objetivo.position);

            var camino = Pathfinder.Dijkstra(origen, destino);
            visualizador.Dibujar(camino);
        }
    }
}

