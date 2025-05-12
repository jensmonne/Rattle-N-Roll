using UnityEngine;
using YawVR;

public class DebugYaw : MonoBehaviour
{
    public void ForceStart()
    {
        YawController.Instance().StartDevice(
            () => Debug.Log("Started successfully"),
            (error) => Debug.LogError("Failed to start device: " + error)
        );
    }

    public void CheckState()
    {
        var currentState = YawController.Instance().State;
        Debug.Log(currentState);
    }
}