using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class ResetView : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    
    [SerializeField] private Transform xrOrigin;
    [SerializeField] private Transform headset;
    
    private InputAction resetViewButton;

    private void OnEnable()
    {
        resetViewButton = inputActions.FindActionMap("LeftHandle").FindAction("ToggleSmartCamera");
        resetViewButton.performed += ResetHeadsetView;
        resetViewButton.Enable();
    }

    private void OnDisable()
    {
        resetViewButton.performed -= ResetHeadsetView;
        resetViewButton.Disable();
    }
    
    private void ResetHeadsetView(InputAction.CallbackContext context)
    {
        Vector3 forward = headset.forward;
        forward.y = 0;
        forward.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(forward);
        xrOrigin.rotation = targetRotation;

        xrOrigin.position = new Vector3(-headset.localPosition.x, xrOrigin.position.y, -headset.localPosition.z);

        Debug.Log("XR view manually recentered.");
    }
}
