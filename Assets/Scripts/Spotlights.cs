using UnityEngine;
using UnityEngine.InputSystem;

public class Spotlights : MonoBehaviour
{
    [SerializeField] private GameObject spotlights;

    [SerializeField] private InputActionAsset inputActions;
    
    private InputAction headlightsOn;
    private InputAction headlightsOff;

    private void OnEnable()
    {
        headlightsOn = inputActions.FindActionMap("SwitchPanel").FindAction("NavLightsOn");
        headlightsOff = inputActions.FindActionMap("SwitchPanel").FindAction("NavLightsOff");
    
        headlightsOn.performed += OnHeadlightsOn;
        headlightsOff.performed += OnHeadlightsOff;

        headlightsOn.Enable();
        headlightsOff.Enable();
    }

    private void OnDisable()
    {
        headlightsOn.performed -= OnHeadlightsOn;
        headlightsOff.performed -= OnHeadlightsOff;

        headlightsOn.Disable();
        headlightsOff.Disable();
    }

    private void OnHeadlightsOn(InputAction.CallbackContext context)
    {
        spotlights.SetActive(true);
    }
    
    private void OnHeadlightsOff(InputAction.CallbackContext context)
    {
        spotlights.SetActive(false);
    }
}
