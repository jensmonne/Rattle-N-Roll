using UnityEngine;

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
        
    }

    public void OnYawDisconnected()
    {
        
    }

    public void OnYawStateChanged()
    {
        
    }
}
