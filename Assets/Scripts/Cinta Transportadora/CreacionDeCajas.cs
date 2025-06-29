using UnityEngine;
using System.Collections.Generic;

public class CreacionDeCajas : MonoBehaviour
{
    [SerializeField] private int spawnSpeed = 2;
    [SerializeField] private CintaTransportadora cintaTransportadora;

    private readonly Queue<int> colaDeCajas = new();
    private float nextSpawnTime;

    [Header("Prefabs de cajas")]
    [SerializeField] private GameObject caja1;
    [SerializeField] private GameObject caja2;

    private void Start()
    {
        EncolarCajaAleatoria();
    }

    private void Update()
    {
        if (colaDeCajas.Count == 0) return;

        if (Time.time >= nextSpawnTime)
        {
            int tipoCaja = colaDeCajas.Dequeue();
            SpawnCaja(tipoCaja);
            nextSpawnTime = Time.time + spawnSpeed;
        }
    }

    public void EncolarCajaAleatoria()
    {
        int nuevoTipo = Random.Range(1, 3);
        colaDeCajas.Enqueue(nuevoTipo);
        Debug.Log($"Se añadió una caja tipo {nuevoTipo}. Total en cola: {colaDeCajas.Count}");
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