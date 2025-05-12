using UnityEngine;
using YawVR;

public class YawRotator : MonoBehaviour
{
    private float angle = 0f;

    void Update()
    {
        // Slowly increment yaw angle (e.g., 30 degrees/sec)
        angle += 30f * Time.deltaTime;
        if (angle > 360f) angle -= 360f;

        // Send to YawController's referenceTransform
        if (YawController.Instance() != null)
        {
            Quaternion rotation = Quaternion.Euler(0, angle, 0); // Only Yaw
            YawController.Instance().TrackerObject.transform.rotation = rotation;
        }
    }
}