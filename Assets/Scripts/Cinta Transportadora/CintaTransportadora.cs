using UnityEngine;
using System.Collections.Generic;

public class CintaTransportadora : MonoBehaviour
{
    [SerializeField] private LinkedList<GameObject> cajasADesplazar = new LinkedList<GameObject>();

    [Header("Movimiento de las cajas")]
    [SerializeField] private Transform puntoInicial;
    [SerializeField] private Transform puntoFinal;

    [SerializeField] private float movementSpeed = 1f;

    private void Update()
    {
        DesplazarCajas();
    }

    public void AgregarCaja(GameObject cajaPrefab)
    {
        GameObject nuevaCaja = Instantiate(cajaPrefab, puntoInicial.position, Quaternion.identity);
        cajasADesplazar.AddLast(nuevaCaja);
        Debug.Log("Se agregó una caja a la cinta.");
    }

    private void DesplazarCajas()
    {
        if (cajasADesplazar.Count == 0)
            return;

        var nodoActual = cajasADesplazar.First;

        while (nodoActual != null)
        {
            GameObject caja = nodoActual.Value;
            if (caja != null)
            {
                caja.transform.position = Vector3.MoveTowards(caja.transform.position, puntoFinal.position, movementSpeed * Time.deltaTime);
                caja.layer = LayerMask.NameToLayer("Default");

                if (Vector3.Distance(caja.transform.position, puntoFinal.position) < 0.1f)
                {
                    var siguienteNodo = nodoActual.Next;
                    cajasADesplazar.Remove(nodoActual);
                    nodoActual = siguienteNodo;
                    caja.layer = LayerMask.NameToLayer("Grabbable");
                    continue;
                }
            }

            nodoActual = nodoActual.Next;
        }
    }
}