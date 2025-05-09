using UnityEngine;
using UnityEngine.InputSystem;

public class Engine : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;
    private InputAction engineOn;
    private InputAction engineOff;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip engineStartClip;
    [SerializeField] private AudioClip engineLoopClip;
    [SerializeField] private AudioClip engineStopClip;
    
    [Header("Lights")]
    [SerializeField] private GameObject[] lights;
    
    private bool[] lightStates;
    
    public static bool isEngineRunning = false;
    
    private void OnEnable()
    {
        engineOn = inputActions.FindActionMap("SwitchPanel").FindAction("MasterBatteryOn");
        engineOff = inputActions.FindActionMap("SwitchPanel").FindAction("MasterBatteryOff");
        
        engineOn.performed += OnEngineOn;
        engineOff.performed += OnEngineOff;

        engineOn.Enable();
        engineOff.Enable();
        
        lightStates = new bool[lights.Length];
    }

    private void OnDisable()
    {
        engineOn.performed -= OnEngineOn;
        engineOff.performed -= OnEngineOff;
        
        engineOn.Disable();
        engineOff.Disable();
    }

    private void OnEngineOn(InputAction.CallbackContext context)
    {
        if (isEngineRunning) return;
        
        isEngineRunning = true;
        
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = engineStartClip;
        audioSource.Play();
        
        Invoke(nameof(PlayLoopingEngineSound), engineStartClip.length);
        
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(lightStates[i]);
        }
    }
    
    private void PlayLoopingEngineSound()
    {
        if (!isEngineRunning) return;

        audioSource.clip = engineLoopClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void OnEngineOff(InputAction.CallbackContext context)
    {
        if (!isEngineRunning) return;
        
        isEngineRunning = false;

        for (int i = 0; i < lights.Length; i++)
        {
            lightStates[i] = lights[i].activeSelf;
            lights[i].SetActive(false);
        }
        
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = engineStopClip;
        audioSource.Play();
    }
}
