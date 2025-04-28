using UnityEngine;
using System.Collections;

public class CamionController : MonoBehaviour
{
    private int cajasRecibidas = 0;
    [SerializeField] private int cajasNecesarias = 5;
    [SerializeField] private float delayAntesDeDestruir = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            cajasRecibidas++;

            other.gameObject.layer = LayerMask.NameToLayer("Default");

            other.tag = "Untagged";

            if (cajasRecibidas >= cajasNecesarias)
            {
                StartCoroutine(DestruirConDelay());
            }
        }
    }

    private IEnumerator DestruirConDelay()
    {
        yield return new WaitForSeconds(delayAntesDeDestruir);

        if (CamionManager.Instance != null)
        {
            CamionManager.Instance.EliminarCamion(gameObject);
        }
        else
        {
            Debug.LogError("❌ CamionManager no encontrado.");
        }

        Destroy(gameObject);
    }
  

}
