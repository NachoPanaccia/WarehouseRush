using System.Collections.Generic;
using UnityEngine;

public class PilaDeCamiones
{
    private Stack<GameObject> pila = new Stack<GameObject>();

    public void Apilar(GameObject camion)
    {
        pila.Push(camion);
    }

    public GameObject Desapilar()
    {
        return pila.Count > 0 ? pila.Pop() : null;
    }

    public bool EstaVacia()
    {
        return pila.Count == 0;
    }
}
