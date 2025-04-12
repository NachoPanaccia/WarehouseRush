using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private float rotationSpeed = 130f;

    void Start()
    {
        
    }
    
    void Update()
    {
        MovementAndRotation();
    }

    private void MovementAndRotation()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(verticalInput * movementSpeed * Time.deltaTime, 0, 0);
        transform.Rotate(0, horizontalInput * rotationSpeed * Time.deltaTime, 0);
    }
}
