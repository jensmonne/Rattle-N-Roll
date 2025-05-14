using UnityEngine;
using YawVR;

public class YawConnector : MonoBehaviour, YawControllerDelegate
{
    private void Start() {
        YawController.Instance().ControllerDelegate = this;
        Debug.Log("Assigned ControllerDelegate");
    }

    public void ControllerStateChanged(ControllerState state) {
        Debug.Log("Controller state changed: " + state);
    }

    public void DidFoundDevice(YawDevice device) {
        Debug.Log("Found device: " + device.Name);
        if (YawController.Instance().State == ControllerState.Initial &&
            (device.Status == DeviceStatus.Available || device.Status == DeviceStatus.Unknown)) {
            YawController.Instance().ConnectToDevice(device,
                () => Debug.Log("Auto-connected to device"),
                (err) => Debug.LogError("Auto-connect failed: " + err));
        }
    }

    public void DidDisconnectFrom(YawDevice device) {
        Debug.Log("Disconnected from: " + device.Name);
    }

    public void DeviceStoppedFromApp() {
        Debug.Log("Device stopped from app");
    }

    public void DeviceStartedFromApp() {
        Debug.Log("Device started from app");
    }
}
