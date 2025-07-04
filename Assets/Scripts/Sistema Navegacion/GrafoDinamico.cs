using System.Collections.Generic;
using UnityEngine;

public class GrafoDinamico : MonoBehaviour
{
    public Vector2Int tamaņoGrilla = new Vector2Int(20, 20);
    public float tamaņoCelda = 1f;
    public LayerMask capaObstaculo;
    public Transform jugador;

    private NodoGrafo[,] nodos;

    public void GenerarGrafo()
    {
        nodos = new NodoGrafo[tamaņoGrilla.x, tamaņoGrilla.y];
        Vector3 origen = jugador.position - new Vector3(tamaņoGrilla.x, 0, tamaņoGrilla.y) * tamaņoCelda * 0.5f;

        for (int x = 0; x < tamaņoGrilla.x; x++)
        {
            for (int y = 0; y < tamaņoGrilla.y; y++)
            {
                Vector3 pos = origen + new Vector3(x * tamaņoCelda, 0, y * tamaņoCelda);
                bool bloqueado = Physics.CheckBox(pos, Vector3.one * tamaņoCelda * 0.4f, Quaternion.identity, capaObstaculo);

                var nodo = new NodoGrafo
                {
                    posicion = pos,
                    caminable = !bloqueado
                };
                nodos[x, y] = nodo;
            }
        }

        // Conectar vecinos
        for (int x = 0; x < tamaņoGrilla.x; x++)
        {
            for (int y = 0; y < tamaņoGrilla.y; y++)
            {
                NodoGrafo nodo = nodos[x, y];
                if (!nodo.caminable) continue;

                AgregarVecino(nodo, x - 1, y);
                AgregarVecino(nodo, x + 1, y);
                AgregarVecino(nodo, x, y - 1);
                AgregarVecino(nodo, x, y + 1);
            }
        }
    }

    void AgregarVecino(NodoGrafo nodo, int x, int y)
    {
        if (x >= 0 && x < tamaņoGrilla.x && y >= 0 && y < tamaņoGrilla.y)
        {
            NodoGrafo vecino = nodos[x, y];
            if (vecino.caminable)
                nodo.vecinos.Add(vecino);
        }
    }

    public NodoGrafo BuscarMasCercano(Vector3 posicion)
    {
        NodoGrafo masCercano = null;
        float minDist = float.MaxValue;

        foreach (var nodo in nodos)
        {
            if (!nodo.caminable) continue;
            float dist = Vector3.Distance(nodo.posicion, posicion);
            if (dist < minDist)
            {
                minDist = dist;
                masCercano = nodo;
            }
        }
        return masCercano;
    }
}

