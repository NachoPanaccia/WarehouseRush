using UnityEngine;

public class CreacionDeCajas : MonoBehaviour
{
    [SerializeField] private Transform boxSpawnPosition;
    [SerializeField] private int spawnSpeed;
    [SerializeField] private int cantidadDeCajas;
    private int[] arrayCajas;

    private float nextSpawnTime = 0f;
    private int indexActual;

    [Header("Posibles Cajas")]
    [SerializeField] private GameObject caja1;
    [SerializeField] private GameObject caja2;


    void Start()
    {
        ColaDeCajas();
    }

    void Update()
    {
        if (indexActual >= arrayCajas.Length)
            return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnCaja(arrayCajas[indexActual]);
            nextSpawnTime = Time.time + spawnSpeed;
            indexActual++;
        }
    }

    void ColaDeCajas()
    {
        indexActual = 0;
        arrayCajas = new int[cantidadDeCajas];

        for (int i = 0; i < cantidadDeCajas; i++)
        {
            arrayCajas[i] = Random.Range(1, 3);
        }
    }

    void SpawnCaja(int tipoCaja)
    {
        GameObject cajaPrefab = null;

        if (tipoCaja == 1)
        {
            cajaPrefab = caja1;
        }
        else if (tipoCaja == 2)
        {
            cajaPrefab = caja2;
        }

        if (cajaPrefab != null)
            Instantiate(cajaPrefab, boxSpawnPosition.position, Quaternion.identity);
    }
}
