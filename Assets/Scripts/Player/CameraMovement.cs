using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 140f;
    [SerializeField] private float verticalClamp = 60f;
    [SerializeField] private float horizontalClamp = 60f;

    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;
    private Quaternion initialRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        horizontalRotation += mouseX;
        verticalRotation -= mouseY;

        horizontalRotation = Mathf.Clamp(horizontalRotation, -horizontalClamp, horizontalClamp);
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalClamp, verticalClamp);

        Quaternion finalRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
        transform.localRotation = initialRotation * finalRotation;
    }
}
