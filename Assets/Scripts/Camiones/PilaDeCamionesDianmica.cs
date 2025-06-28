using UnityEngine;

public class NodoPilaCamion
{
    public GameObject dato;
    public NodoPilaCamion siguiente;
}

public class PilaDeCamionesDinamica
{
    private NodoPilaCamion tope;

    public void InicializarPila()
    {
        tope = null;
    }

    public void Apilar(GameObject camion)
    {
        NodoPilaCamion nuevo = new NodoPilaCamion();
        nuevo.dato = camion;
        nuevo.siguiente = tope;
        tope = nuevo;
    }

    public GameObject Desapilar()
    {
        if (tope != null)
        {
            GameObject temp = tope.dato;
            tope = tope.siguiente;
            return temp;
        }
        return null;
    }

    public GameObject Tope()
    {
        return tope != null ? tope.dato : null;
    }

    public bool PilaVacia()
    {
        return tope == null;
    }
}