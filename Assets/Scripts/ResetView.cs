using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class ResetView : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    [SerializeField] private Transform xrOrigin;
    public Transform target;
    public Transform cameraTransform; 
    
    
    

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
        
        if (xrOrigin == null)
        {
            Debug.LogWarning("XR Origin not assigned.");
            return;
        }

        xrOrigin.position = target.position;
        cameraTransform.Rotate(0, 85, 0);
        
        Debug.LogError("View has been reset");
    }
}

