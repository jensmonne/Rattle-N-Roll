using UnityEngine;
using UnityEngine.InputSystem;
using YawVR;

public class CarController : MonoBehaviour
{
    [SerializeField] private Transform steeringWheel;
    [SerializeField] private DashBoardController dashController;
    //[SerializeField] private AudioSource grindingAudioSource;
    //[SerializeField] private AudioClip gearGrindClip;
    
    [Header("Input Actions")]
    [SerializeField] private InputActionReference accelerateInput;
    [SerializeField] private InputActionReference brakeInput;
    [SerializeField] private InputActionReference steeringInput;
    //[SerializeField] private InputActionReference clutchInput;
    
    [Tooltip("Index 0 = Neutral, 1 = Reverse, 2 = 1st, 3 = 2nd, 4 = 3rd")]
    [SerializeField] private InputActionReference[] gearShiftInputs;
    
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheel;
    [SerializeField] private WheelCollider frontRightWheel;
    [SerializeField] private WheelCollider rearLeftWheel;
    [SerializeField] private WheelCollider rearRightWheel;
    
    [Header("Settings")]
    [SerializeField] private float[] gearRatios = { 0f, -0.5f, 0.4f, 0.7f, 1.0f };
    [SerializeField] private float maxMotorTorque = 1500f;
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float maxWheelRotation = 90f;
    
    [SerializeField] private int currentGearIndex = 0;
    //private int previousGearIndex = 0;
    //private int attemptedGearIndex = -1;
    //private bool clutchIn = false;
    //private bool isGrinding = false;
    
    private float steerInput;
    private float accelInput;
    private float brakeInputValue;
    
    private void LateUpdate()
    {
        if (YawController.Instance() != null)
        {
            YawController.Instance().TrackerObject.transform.rotation = transform.rotation;
        }
    }
    
    private void OnEnable()
    {
        //clutchInput.action.performed += _ => clutchIn = !clutchIn;
        
        for (int i = 0; i < gearShiftInputs.Length; i++)
        {
            gearShiftInputs[i].action.Enable();
            int gear = i;
            gearShiftInputs[i].action.performed += ctx => SetGear(gear);
        }
    }

    private void OnDisable()
    {
        //clutchInput.action.performed -= _ => clutchIn = !clutchIn;
        
        for (int i = 0; i < gearShiftInputs.Length; i++)
        {
            gearShiftInputs[i].action.performed -= ctx => SetGear(i);
            gearShiftInputs[i].action.Disable();
        }
    }

    private void FixedUpdate()
    {
        if (!Engine.isEngineRunning) return;

        //if (isGrinding) return;
        
        steerInput = steeringInput.action.ReadValue<float>();
        accelInput = accelerateInput.action.ReadValue<float>();
        brakeInputValue = brakeInput.action.ReadValue<float>();

        float visualRotation = steerInput * maxWheelRotation * 0.5f;
        steeringWheel.localRotation = Quaternion.Euler(0f, 0f, -visualRotation);
        float steerAngle = steerInput * maxSteerAngle;
        frontLeftWheel.steerAngle = steerAngle;
        frontRightWheel.steerAngle = steerAngle;

        float gearRatio = gearRatios[currentGearIndex];
        float motor = accelInput * maxMotorTorque * gearRatio;
        
        rearLeftWheel.motorTorque = motor;
        rearRightWheel.motorTorque = motor;

        float brakeForce = brakeInputValue * maxMotorTorque;
        rearLeftWheel.brakeTorque = brakeForce;
        rearRightWheel.brakeTorque = brakeForce;
    }

    private void DrainFuel(float motorTorque)
    {
        if (motorTorque == 0) return;
    }

    private void SetGear(int gearIndex)
    {
        if (gearIndex < 0 || gearIndex >= gearRatios.Length)
        {
            Debug.LogWarning("Invalid gear index");
            return;
        }
        
        if (!Engine.isEngineRunning) return;
        
        /*if (!clutchIn && !isGrinding)
        {
            attemptedGearIndex = gearIndex;
            PlayGrindingSound();
            return;
        }
        
        // NOTE: this shit has to be checked, i cant get a steering wheel outta my ass so have to check at school 
        if (isGrinding && clutchIn && gearIndex == attemptedGearIndex)
        {
            StopGrindingSound();
        }
        
        if (isGrinding && (gearIndex == previousGearIndex || gearIndex == 0))
        {
            StopGrindingSound();
        }

        previousGearIndex = currentGearIndex;*/
        currentGearIndex = gearIndex;
        //attemptedGearIndex = -1;
        dashController.SetGearMessage(GearLabel(currentGearIndex));
        Debug.Log($"Gear changed to: {GearLabel(currentGearIndex)}");
    }

    private string GearLabel(int index)
    {
        return index switch
        {
            0 => "Neutral",
            1 => "Reverse",
            2 => "1st",
            3 => "2nd",
            4 => "3rd",
            _ => "Unknown"
        };
    }

    /*private void PlayGrindingSound()
    {
        if (grindingAudioSource && gearGrindClip)
        {
            grindingAudioSource.clip = gearGrindClip;
            grindingAudioSource.loop = true;
            grindingAudioSource.Play();
            isGrinding = true;
        }
    }

    private void StopGrindingSound()
    {
        if (grindingAudioSource && grindingAudioSource.isPlaying)
        {
            grindingAudioSource.Stop();
            isGrinding = false;
        }
    }*/
}
