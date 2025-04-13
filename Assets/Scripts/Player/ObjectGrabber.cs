using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float grabDistance = 3f;
    [SerializeField] private float grabSmoothness = 10f;
    [SerializeField] private LayerMask grabbableLayer;

    private Rigidbody grabbedObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryGrabObject();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseObject();
        }

        if (grabbedObject != null)
        {
            MoveObject();
        }
    }

    private void TryGrabObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, grabbableLayer))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;
            if (rb != null)
            {
                grabbedObject = rb;
                grabbedObject.useGravity = false;
                grabbedObject.linearDamping = 10f; // Para que no se descontrole
            }
        }
    }

    private void MoveObject()
    {
        Vector3 targetPos = playerCamera.transform.position + playerCamera.transform.forward * grabDistance;
        Vector3 moveDirection = (targetPos - grabbedObject.position);
        grabbedObject.linearVelocity = moveDirection * grabSmoothness;
    }

    private void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.useGravity = true;
            grabbedObject.linearDamping = 0f;
            grabbedObject = null;
        }
    }
}
