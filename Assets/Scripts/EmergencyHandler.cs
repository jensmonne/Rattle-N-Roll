using System.Collections;
using UnityEngine;
using YawVR;

public class EmergencyHandler : MonoBehaviour
{
    public static EmergencyHandler Instance { get; private set; }
    
    [Header("Buzzer Settings")]
    [SerializeField] private int buzzerAmplitude = 255;
    [SerializeField] private int buzzerFrequency = 100;
    [SerializeField] private float warningDuration = 2.0f;

    [Header("LED Flash Settings")]
    [SerializeField] private Color32 flashColor = new Color32(255, 0, 0, 255);
    [SerializeField] private float flashInterval = 0.3f;
    [SerializeField] private int flashCount = 25;
    
    private Coroutine ledFlashCoroutine;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void TriggerEmergencyStop(bool quitAfter = true)
    {
        if (YawController.Instance() == null || YawController.Instance().State == ControllerState.Stopping)
        {
            Debug.LogWarning("Yaw is not running. Emergency stop skipped.");
            if (quitAfter) Application.Quit();
            return;
        }

        StartEmergencyEffects();

        YawController.Instance().StopDevice(
            park: false,
            onSuccess: () =>
            {
                StopEmergencyEffects();
                Debug.Log("Yaw emergency-stopped (ABORT)");
                if (quitAfter) Application.Quit();
            },
            onError: error =>
            {
                StopEmergencyEffects();
                Debug.LogError("Emergency stop failed: " + error);
                if (quitAfter) Application.Quit();
            }
        );
    }
    
    private void StartEmergencyEffects()
    {
        var buzzer = YawController.Instance().Buzzer;
        buzzer.SetBuzzerAmps(buzzerAmplitude, buzzerAmplitude, buzzerAmplitude);
        buzzer.SetHz(buzzerFrequency);
        buzzer.SetOn(true);

        ledFlashCoroutine = StartCoroutine(FlashLED());
    }
    
    private void StopEmergencyEffects()
    {
        YawController.Instance().Buzzer.SetOn(false);
        if (ledFlashCoroutine != null) StopCoroutine(ledFlashCoroutine);
        YawController.Instance().SendLED(new Color32(0, 0, 0, 255));
    }
    
    private IEnumerator FlashLED()
    {
        for (int i = 0; i < flashCount; i++)
        {
            YawController.Instance().SendLED(flashColor);
            yield return new WaitForSeconds(flashInterval);
            YawController.Instance().SendLED(new Color32(0, 0, 0, 255));
            yield return new WaitForSeconds(flashInterval);
        }
    }
}
