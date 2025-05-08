using UnityEngine;
using UnityEngine.InputSystem;

public class DrivingTest : MonoBehaviour
{
    [SerializeField] private InputActionReference moveForward;
    [SerializeField] private InputActionReference moveBackward;
    [SerializeField] private InputActionReference steer;
    private float moveSpeed = 5f;
    private float turnSpeed = 100f;

    private void Update()
    {
        float forwardInput = moveForward.action.ReadValue<float>();
        float BackInput = moveBackward.action.ReadValue<float>();
        float inputZ = forwardInput - BackInput;
        
        float inputX = steer.action.ReadValue<float>();
        
        float rotation = inputX * turnSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
        
        Vector3 move = transform.forward * (inputZ * moveSpeed * Time.deltaTime);
        transform.Translate(move, Space.World);
    }
}
