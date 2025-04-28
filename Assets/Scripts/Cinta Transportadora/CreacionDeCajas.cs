using UnityEngine;
using System.Collections.Generic;

public class CreacionDeCajas : MonoBehaviour
{
    [SerializeField] private int spawnSpeed = 2;
    [SerializeField] private int cantidadDeCajas = 10;
    [SerializeField] private CintaTransportadora cintaTransportadora;

    private Queue<int> colaDeCajas;
    private float nextSpawnTime = 0f;

    [Header("Prefabs de cajas")]
    [SerializeField] private GameObject caja1;
    [SerializeField] private GameObject caja2;

    private void Start()
    {
        GenerarColaDeCajas();
    }

    private void Update()
    {
        if (colaDeCajas.Count == 0)
            return;

        if (Time.time >= nextSpawnTime)
        {
            int tipoCaja = colaDeCajas.Dequeue();
            SpawnCaja(tipoCaja);
            nextSpawnTime = Time.time + spawnSpeed;
        }
    }

    private void GenerarColaDeCajas()
    {
        colaDeCajas = new Queue<int>();

        for (int i = 0; i < cantidadDeCajas; i++)
        {
            colaDeCajas.Enqueue(Random.Range(1, 3));
        }
    }

    private void SpawnCaja(int tipoCaja)
    {
        GameObject cajaPrefab = null;

        switch (tipoCaja)
        {
            case 1:
                cajaPrefab = caja1;
                break;
            case 2:
                cajaPrefab = caja2;
                break;
        }

        if (cajaPrefab != null)
        {
            cintaTransportadora.AgregarCaja(cajaPrefab);
        }
    }
}