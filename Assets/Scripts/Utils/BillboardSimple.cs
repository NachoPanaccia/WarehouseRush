using UnityEngine;

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
