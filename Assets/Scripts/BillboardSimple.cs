using UnityEngine;

/// <summary>
/// Hace que el objeto mire siempre a la c�mara principal.
/// �til para r�tulos 3D (TextMeshPro).
/// </summary>
public class BillboardSimple : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main == null) return;
        Vector3 dir = transform.position - Camera.main.transform.position;
        if (dir.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(dir);
    }
}
