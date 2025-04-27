using UnityEngine;
using System.Collections.Generic;

public class CintaTransportadora : MonoBehaviour
{
    private int movementSpeed;
    private LinkedList<GameObject> cajasADesplazar;

    [Header("Puntos de movimiento")]
    [SerializeField] private Transform puntoInicial;
    [SerializeField] private Transform puntoFinal;

    private void Start()
    {
        cajasADesplazar = new LinkedList<GameObject>();
    }
}
