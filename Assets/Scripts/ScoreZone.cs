using UnityEngine;
using System.Collections;

public class ScoreZone : MonoBehaviour
{
    public string validBoxTag = "Box";
    private int cajasRecibidas = 0;
    [SerializeField] private float delayAntesDeDestruir = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(validBoxTag))
        {
            cajasRecibidas++;

            // Hacemos hijo el objeto dentro del camión
            other.transform.SetParent(transform);

            if (cajasRecibidas >= 2)
            {
                StartCoroutine(DestruirConDelay());
            }
        }
    }

    private IEnumerator DestruirConDelay()
    {
        Debug.Log("Camión completo. Destruyendo en " + delayAntesDeDestruir + " segundos...");
        yield return new WaitForSeconds(delayAntesDeDestruir);
        Destroy(gameObject);
    }
}

