using TMPro;
using UnityEngine;

public class DebugSpeed : MonoBehaviour
{
    [SerializeField] private Rigidbody carRigidbody;
    [SerializeField] private TMP_Text speedText;

    private float currentSpeed = 0;

    private void Update()
    {
        if (!Engine.isEngineRunning) return;
        
        currentSpeed = carRigidbody.linearVelocity.magnitude * 3.6f;
        
        speedText.text = $"{currentSpeed:F2} km/h";
    }
}
