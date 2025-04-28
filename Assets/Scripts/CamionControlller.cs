using UnityEngine;
using System.Collections;

public class CamionController : MonoBehaviour
{
    private int cajasRecibidas = 0;
    [SerializeField] private int cajasNecesarias = 2;
    [SerializeField] private float delayAntesDeDestruir = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            cajasRecibidas++;

            other.gameObject.layer = LayerMask.NameToLayer("Default");
            other.transform.SetParent(transform, true);

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
