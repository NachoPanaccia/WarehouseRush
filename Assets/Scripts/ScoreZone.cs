using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    public string validBoxTag = "Box"; // <- etiqueta de la caja correcta

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(validBoxTag))
        {
            ScoreManager.Instance.AddPoint();
            Debug.Log("¡Caja entregada!");
        }
    }
}

