using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using YawVR;

public class YawEvents : MonoBehaviour
{
    private static YawEvents Instance { get; set; }

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

    public void OnYawConnected()
    {
        SetYawToPark();
    }

    public void OnYawDisconnected()
    {
        
    }

    public void OnYawStateChanged()
    {
        
    }

    private void SetYawToPark()
    {
        YawController.Instance().StartDevice(
            () => Debug.Log("Yaw started"),
            error => Debug.LogError("Failed to start device: " + error)
        );
        
        YawController.Instance().StopDevice(
            true,
            () => Debug.Log("Yaw stopped"),
            error => Debug.LogError("Failed to stop device: " + error)
        );
    }
}
