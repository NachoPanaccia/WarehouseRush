using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathVisualizer : MonoBehaviour
{
    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public void Dibujar(List<NodoGrafo> camino)
    {
        line.positionCount = camino.Count;
        for (int i = 0; i < camino.Count; i++)
        {
            line.SetPosition(i, camino[i].posicion + Vector3.up * 0.1f);
        }
    }
}
