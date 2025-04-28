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

            
            other.transform.SetParent(transform, true);

            if (cajasRecibidas >= 2)
            {
                StartCoroutine(DestruirConDelay());
            }
        }
    }

    private IEnumerator DestruirConDelay()
    {
       
        yield return new WaitForSeconds(delayAntesDeDestruir);

        
        if (CamionManager.Instance != null && transform.root.gameObject != null)
        {
            CamionManager.Instance.EliminarCamion(transform.root.gameObject);
            Destroy(transform.root.gameObject);
        }
        else
        {
            Debug.LogError("ERRRRRROR");
        }
    }

}


