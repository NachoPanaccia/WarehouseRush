using UnityEngine;
using System.Collections.Generic;

public class NodoGrafo 
{
    public Vector3 posicion;
    public bool caminable;
    public List<NodoGrafo> vecinos = new List<NodoGrafo>();

    public float distancia = float.MaxValue;
    public NodoGrafo previo = null;
}