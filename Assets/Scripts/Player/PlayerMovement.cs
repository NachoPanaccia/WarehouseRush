using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private float rotationSpeed = 130f;
    [SerializeField] private Transform respawnPoint;

    private float maxTiltAngle = 70f;
    private float tiltDurationNeeded = 3;
    private float tiltTimer = 0f; 

    void Update()
    {
        MovementAndRotation();
        CheckTiltAndRespawn();
    }

    private void MovementAndRotation()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(verticalInput * movementSpeed * Time.deltaTime, 0, 0);
        transform.Rotate(0, horizontalInput * rotationSpeed * Time.deltaTime, 0);
    }

    private void CheckTiltAndRespawn()
    {
        float currentTilt = Vector3.Angle(transform.up, Vector3.up);
        bool isTilted = currentTilt > maxTiltAngle;

        if (isTilted)
        {
            tiltTimer += Time.deltaTime;
            if (tiltTimer >= tiltDurationNeeded)
                Respawn();
        }
        else
        {
            tiltTimer = 0f;
        }
    }

    private void Respawn()
    {
        transform.position = respawnPoint.position;

        Vector3 yOnly = new Vector3(0, respawnPoint.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(yOnly);

        tiltTimer = 0f;
    }
}
