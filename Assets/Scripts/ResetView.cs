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
        Vector3 headsetForward = headset.forward;
        headsetForward.y = 0; // Flatten to horizontal
        headsetForward.Normalize();

        // Find the angle between where the headset is looking and the car's forward
        float angleOffset = Vector3.SignedAngle(headsetForward, xrOrigin.forward, Vector3.up);

        // Apply the inverse of that angle to the XR Origin
        xrOrigin.Rotate(0, angleOffset, 0, Space.Self);

        Debug.Log("Recentered headset view relative to XR Origin.");
    }
}
