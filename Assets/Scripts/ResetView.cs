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
        
        Transform cameraTransform = Camera.main.transform;

        Vector3 cameraOffset = cameraTransform.position - xrOrigin.position;
        
        Quaternion currentCameraRotation = cameraTransform.rotation;
        Quaternion targetRotation = target.rotation;

        Quaternion rotationDelta = targetRotation * Quaternion.Inverse(currentCameraRotation);

        xrOrigin.rotation = rotationDelta * xrOrigin.rotation;

        cameraOffset = cameraTransform.position - xrOrigin.position;

        xrOrigin.position = target.position - cameraOffset;
        
        Debug.LogError("View has been reset");
    }
}

