using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class ResetView : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    
    [SerializeField] private Transform xrOrigin;
    //[SerializeField] private Transform headset;
    
    [SerializeField] private Vector3 seatPosition = new Vector3(-0.33f, 0.63f, 0.0509f);
    [SerializeField] private Vector3 seatRotation = Vector3.zero;
    
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

        xrOrigin.localPosition = seatPosition;
        xrOrigin.localRotation = Quaternion.Euler(seatRotation);

        Debug.Log("XR Origin reset to seat position and rotation.");
    }
}
