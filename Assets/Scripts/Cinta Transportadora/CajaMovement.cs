using UnityEngine;

public class CajaMovement : MonoBehaviour
{
    [SerializeField] private float cajaSpeed;

    void Update()
    {

    }

    public void Desplazamiento()
    {
        transform.Translate(Vector3.forward * cajaSpeed * Time.deltaTime);
    }
}
