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

            // SOLUCIÓN: mantener la posición global
            other.transform.SetParent(transform, true);

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


