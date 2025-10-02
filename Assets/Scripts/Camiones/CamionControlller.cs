using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class CamionController : MonoBehaviour
{
    [SerializeField] private string tagCaja = "Box"; // si tu tag es "box", cambialo acá
    [SerializeField] private int cajasNecesarias = 5;
    [SerializeField] private float delayAntesDeDespachar = 2f;

    private int cajasRecibidas = 0;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tagCaja)) return;

        cajasRecibidas++;

        other.gameObject.layer = LayerMask.NameToLayer("Default");
        other.tag = "Untagged";
        other.transform.SetParent(transform, true);

        if (cajasRecibidas >= cajasNecesarias)
            StartCoroutine(DespacharTrasDelay());
    }

    private IEnumerator DespacharTrasDelay()
    {
        yield return new WaitForSeconds(delayAntesDeDespachar);

        if (CamionManager.Instance != null)
            CamionManager.Instance.NotificarCamionListo(this);
        else
            Debug.LogError("CamionManager no encontrado en escena.");
    }
}
