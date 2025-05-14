using System.Collections;
using UnityEngine;
using YawVR;

public class YawStartupActions : MonoBehaviour
{
    private static YawStartupActions Instance { get; set; }

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

    private void Start()
    {
        StartCoroutine(ForceRedLED()); // Flash red for 0.5 seconds
    }

    IEnumerator FlashRedLED(float duration = 1f)
    {
        // Flash RED
        YawController.Instance().SendLED(new Color32(255, 0, 0, 255));
        yield return new WaitForSeconds(duration);

        // Turn OFF (black)
        YawController.Instance().SendLED(new Color32(0, 0, 0, 0));
    }
    
    IEnumerator ForceRedLED()
    {
        while (YawController.Instance().State == ControllerState.Started)
        {
            YawController.Instance().SendLED(new Color32(255, 0, 0, 255));
            yield return new WaitForSeconds(0.5f);
        }
    }
}