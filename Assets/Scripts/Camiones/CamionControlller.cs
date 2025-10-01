using UnityEngine;
using System.Collections;

public class CamionController : MonoBehaviour
{
    private int cajasRecibidas = 0;
    [SerializeField] private int cajasNecesarias = 5;
    [SerializeField] private float delayAntesDeDestruir = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Box")) return;

        cajasRecibidas++;

        other.gameObject.layer = LayerMask.NameToLayer("Default");
        other.tag = "Untagged";
        other.transform.SetParent(transform, true);

        if (cajasRecibidas >= cajasNecesarias)
            StartCoroutine(DestruirConDelay());
    }

    private IEnumerator DestruirConDelay()
    {
        yield return new WaitForSeconds(delayAntesDeDestruir);
        if (CamionManager.Instance != null)
            CamionManager.Instance.EliminarCamion(gameObject);
        else
            Debug.LogError("CamionManager no encontrado.");
        // La destrucción final siempre la hace el manager al desapilar
    }
}
