using System;
using TMPro;
using UnityEngine;

public class DashBoardController : MonoBehaviour
{
    [SerializeField] private GameObject HUD;
    [SerializeField] private TMP_Text gearText;
    [SerializeField] private Rigidbody carRigidbody;
    [SerializeField] private TMP_Text speedText;
    
    private float currentSpeed = 0;
    public void SetGearMessage(string currentGear)
    {
        char firstChar = currentGear[0];
        gearText.text = firstChar.ToString();
    }

    public void Update()
    {
        if (!Engine.isEngineRunning) return;
        
        currentSpeed = carRigidbody.linearVelocity.magnitude * 3.6f;
        
        speedText.text = $"{currentSpeed:F2}";
    }
}
