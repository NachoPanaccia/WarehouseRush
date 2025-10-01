using UnityEngine;

public class NodoPilaCamion
{
    public GameObject dato;
    public NodoPilaCamion siguiente;
}

public class PilaDeCamionesDinamica
{
    private NodoPilaCamion tope;
    public int Count { get; private set; }

    public void InicializarPila()
    {
        tope = null;
        Count = 0;
    }

    public void Apilar(GameObject camion)
    {
        var nuevo = new NodoPilaCamion { dato = camion, siguiente = tope };
        tope = nuevo;
        Count++;
    }

    public GameObject Desapilar()
    {
        if (tope == null) return null;
        var temp = tope.dato;
        tope = tope.siguiente;
        Count--;
        return temp;
    }

    public GameObject Peek() => tope != null ? tope.dato : null;
    public bool PilaVacia() => tope == null;
}
