using UnityEngine;
using UnityEngine.InputSystem;

public class AccelerateBrake : MonoBehaviour
{
    [SerializeField] private InputActionReference Accelerate;
    [SerializeField] private InputActionReference Deaccelerate;

    [SerializeField] private float maxSpeed = 10f;
    
    private float currentSpeed = 0f;
    
    private void Update()
    {
        float accelerateValue = Accelerate.action.ReadValue<float>();
        float brakeValue = Deaccelerate.action.ReadValue<float>();
        
        float netAcceleration = accelerateValue - brakeValue;
        
        currentSpeed = Mathf.Clamp(currentSpeed + netAcceleration * Time.deltaTime * maxSpeed, 0f, maxSpeed);
        
        transform.Translate(Vector3.forward * (currentSpeed * Time.deltaTime));
    }
}
