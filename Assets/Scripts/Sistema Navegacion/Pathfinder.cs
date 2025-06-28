using System.Collections.Generic;
using UnityEngine;


public class Pathfinder
{
    public static List<NodoGrafo> Dijkstra(NodoGrafo inicio, NodoGrafo destino)
    {
        List<NodoGrafo> abiertos = new List<NodoGrafo>();

        foreach (var nodo in ObtenerTodos(inicio))
        {
            nodo.distancia = float.MaxValue;
            nodo.previo = null;
            abiertos.Add(nodo);
        }

        inicio.distancia = 0;

        while (abiertos.Count > 0)
        {
            abiertos.Sort((a, b) => a.distancia.CompareTo(b.distancia));
            NodoGrafo actual = abiertos[0];
            abiertos.RemoveAt(0);

            foreach (var vecino in actual.vecinos)
            {
                float nuevoDist = actual.distancia + Vector3.Distance(actual.posicion, vecino.posicion);
                if (nuevoDist < vecino.distancia)
                {
                    vecino.distancia = nuevoDist;
                    vecino.previo = actual;
                }
            }
        }

        List<NodoGrafo> camino = new List<NodoGrafo>();
        NodoGrafo nodoActual = destino;
        while (nodoActual != null)
        {
            camino.Insert(0, nodoActual);
            nodoActual = nodoActual.previo;
        }
        return camino;
    }

    private static HashSet<NodoGrafo> ObtenerTodos(NodoGrafo inicio)
    {
        HashSet<NodoGrafo> visitados = new HashSet<NodoGrafo>();
        Queue<NodoGrafo> cola = new Queue<NodoGrafo>();
        cola.Enqueue(inicio);

        while (cola.Count > 0)
        {
            var actual = cola.Dequeue();
            if (!visitados.Contains(actual))
            {
                visitados.Add(actual);
                foreach (var v in actual.vecinos)
                    cola.Enqueue(v);
            }
        }

        return visitados;
    }
}
