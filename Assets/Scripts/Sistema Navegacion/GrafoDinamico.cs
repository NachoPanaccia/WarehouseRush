using System.Collections.Generic;
using UnityEngine;

public class GrafoDinamico : MonoBehaviour
{
    public Vector2Int tama�oGrilla = new Vector2Int(20, 20);
    public float tama�oCelda = 1f;
    public LayerMask capaObstaculo;
    public Transform jugador;

    public NodoGrafo[,] nodos;

    public void GenerarGrafo()
    {
        nodos = new NodoGrafo[tama�oGrilla.x, tama�oGrilla.y];
        Vector3 origen = jugador.position - new Vector3(tama�oGrilla.x, 0, tama�oGrilla.y) * tama�oCelda * 0.5f;

        for (int x = 0; x < tama�oGrilla.x; x++)
        {
            for (int y = 0; y < tama�oGrilla.y; y++)
            {
                Vector3 pos = origen + new Vector3(x * tama�oCelda, 0, y * tama�oCelda);
                bool bloqueado = Physics.CheckBox(pos, Vector3.one * tama�oCelda * 0.4f, Quaternion.identity, capaObstaculo);

                var nodo = new NodoGrafo
                {
                    posicion = pos,
                    caminable = !bloqueado
                };
                nodos[x, y] = nodo;
            }
        }

        // Conectar vecinos
        for (int x = 0; x < tama�oGrilla.x; x++)
        {
            for (int y = 0; y < tama�oGrilla.y; y++)
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
        if (x >= 0 && x < tama�oGrilla.x && y >= 0 && y < tama�oGrilla.y)
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
