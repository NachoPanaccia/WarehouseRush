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
        Debug.Log("⏳ Destrucción iniciada, esperando " + delayAntesDeDestruir + " segundos...");
        yield return new WaitForSeconds(delayAntesDeDestruir);

        // Super Debug
        Debug.Log("⭐ SuperDebug START ⭐");

        if (CamionManager.Instance == null)
        {
            Debug.LogError("❌ CamionManager.Instance ES NULL!!! (No existe el objeto en escena)");
        }
        else
        {
            Debug.Log("✅ CamionManager.Instance encontrado: " + CamionManager.Instance.gameObject.name);
        }

        if (transform == null)
        {
            Debug.LogError("❌ transform ES NULL!!! (El ScoreZone está mal colocado)");
        }
        else
        {
            Debug.Log("✅ Transform correcto: " + transform.name);
        }

        if (transform.root == null)
        {
            Debug.LogError("❌ transform.root ES NULL!!! (El objeto raíz no existe)");
        }
        else
        {
            Debug.Log("✅ transform.root correcto: " + transform.root.name);
        }

        if (transform.root.gameObject == null)
        {
            Debug.LogError("❌ transform.root.gameObject ES NULL!!! (El GameObject raíz no existe)");
        }
        else
        {
            Debug.Log("✅ GameObject raíz correcto: " + transform.root.gameObject.name);
        }

        Debug.Log("⭐ SuperDebug END ⭐");

        // Ahora sí intentar eliminar si todo está bien
        if (CamionManager.Instance != null && transform.root.gameObject != null)
        {
            CamionManager.Instance.EliminarCamion(transform.root.gameObject);
            Destroy(transform.root.gameObject);
        }
        else
        {
            Debug.LogError("❌ No se puede eliminar camión porque falta alguna referencia.");
        }
    }

}


