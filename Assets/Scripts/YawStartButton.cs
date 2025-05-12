using UnityEngine;
using UnityEngine.InputSystem;
using YawVR;

public class YawStartButton : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    
    private InputAction yawOn;
    private InputAction yawOff;

    private void OnEnable()
    {
        yawOn = inputActions.FindActionMap("SwitchPanel").FindAction("AlternatorOn");
        yawOff = inputActions.FindActionMap("SwitchPanel").FindAction("AlternatorOff");
    
        yawOn.performed += OnYawOn;
        yawOff.performed += OnYawOff;

        yawOn.Enable();
        yawOff.Enable();
    }

    private void OnDisable()
    {
        yawOn.performed -= OnYawOn;
        yawOff.performed -= OnYawOff;

        yawOn.Disable();
        yawOff.Disable();
    }

    private void OnYawOn(InputAction.CallbackContext context)
    {
        YawController.Instance().StartDevice(
            () => Debug.Log("Started successfully"),
            (error) => Debug.LogError("Failed to start device: " + error)
        );
    }
    
    private void OnYawOff(InputAction.CallbackContext context)
    {
        YawController.Instance().StopDevice(
            park: true,
            onSuccess: () => Debug.Log("Device stopped successfully"),
            onError: error => Debug.LogError("Failed to stop device: " + error)
        );
    }
}
