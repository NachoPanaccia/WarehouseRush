using UnityEngine;

public class PesoDeCamion : MonoBehaviour
{
    [SerializeField, Tooltip("Se asigna automáticamente en Awake en cada partida.")]
    private int peso;

    public int Peso => peso;

    private void Awake()
    {
        // int: min incl., max excl. => [1..50]
        peso = Random.Range(1, 51);
    }
}
