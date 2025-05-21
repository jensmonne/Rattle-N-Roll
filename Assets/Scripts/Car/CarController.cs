using UnityEngine;
using UnityEngine.InputSystem;
using YawVR;

public class CarController : MonoBehaviour
{
    [SerializeField] private Transform steeringWheel;
    [SerializeField] private DashBoardController dashController;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AnimationCurve torqueCurve;
    [SerializeField] private AudioSource grindingAudioSource;
    [SerializeField] private AudioClip gearGrindClip;
    [SerializeField] private float maxRPM = 7000f;
    [SerializeField] private AudioSource engineAudioSource;
    
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
    [SerializeField] private float[] gearRatios = { 0f, -0.6f, 1.1f, 1.4f, 1.7f };
    private readonly float[] minSpeedsForGears = { 0f, 0f, 0f, 25f, 55f }; // in km/h
    [SerializeField] private float maxMotorTorque = 1500f;
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float maxWheelRotation = 90f;
    [SerializeField] private float finalDriveRatio = 4.1f;
    
    [SerializeField] private int currentGearIndex = 0;
    //private int previousGearIndex = 0;
    //private int attemptedGearIndex = -1;
    //private bool clutchIn = false;
    private bool isGrinding = false;
    
    private float steerInput;
    private float accelInput;
    private float brakeInputValue;
    
    private float engineRPM;
    
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
        if (!Engine.isEngineRunning || isGrinding) return;
        
        float currentSpeedKmh = rb.linearVelocity.magnitude * 3.6f;
        if (currentGearIndex > 1 && currentSpeedKmh < minSpeedsForGears[currentGearIndex])
        {
            Debug.LogWarning($"Too slow for current gear ({GearLabel(currentGearIndex)}), forcing to Neutral");
            grindingAudioSource.clip = gearGrindClip;
            grindingAudioSource.loop = false;
            grindingAudioSource.Play();
            currentGearIndex = 0;
            dashController.SetGearMessage("Neutral");
            return;
        }
        
        float wheelRPM = (rearLeftWheel.rpm + rearRightWheel.rpm) * 0.5f;
        float targetRPM = Mathf.Abs(wheelRPM * gearRatios[currentGearIndex] * finalDriveRatio);
        engineRPM = Mathf.Lerp(engineRPM, targetRPM, 0.1f);
        float normalizedRPM = Mathf.Clamp01(engineRPM / maxRPM);

        steerInput = steeringInput.action.ReadValue<float>();
        accelInput = accelerateInput.action.ReadValue<float>();
        brakeInputValue = brakeInput.action.ReadValue<float>();

        float visualRotation = steerInput * maxWheelRotation * 0.5f;
        steeringWheel.localRotation = Quaternion.Euler(0f, 0f, -visualRotation);
        float steerAngle = steerInput * maxSteerAngle;
        frontLeftWheel.steerAngle = steerAngle;
        frontRightWheel.steerAngle = steerAngle;

        float gearRatio = gearRatios[currentGearIndex];
        float currentSpeed = rb.linearVelocity.magnitude;
        float speedFactor = Mathf.Clamp01(1f - currentSpeed / 50f); // Need to tweak the last float number for top speed stuff
        float torqueMultiplier = torqueCurve.Evaluate(normalizedRPM);
        float motor = accelInput * maxMotorTorque * gearRatio * torqueMultiplier;
        
        rearLeftWheel.motorTorque = motor;
        rearRightWheel.motorTorque = motor;

        float brakeForce = brakeInputValue * maxMotorTorque;
        rearLeftWheel.brakeTorque = brakeForce;
        rearRightWheel.brakeTorque = brakeForce;
        
        engineAudioSource.pitch = Mathf.Lerp(0.8f, 2.0f, normalizedRPM);
        
        DrainFuel(motor);
    }

    private void DrainFuel(float motorTorque)
    {
        float currentFuel = Fuel.currentFuelAmount;
        float maxFuel = Fuel.maxfuelAmount;
            
        if (currentFuel <= 0f)
        {
            Engine.TurnEngineOff();
            Debug.Log("Out of fuel!");
            return;
        }
        
        float drainRate = Mathf.Abs(motorTorque) * 0.005f;
        currentFuel -= drainRate * Time.fixedDeltaTime;

        if (currentFuel < 0f) currentFuel = 0f;

        Fuel.currentFuelAmount = currentFuel;
    }

    private void SetGear(int gearIndex)
    {
        if (gearIndex < 0 || gearIndex >= gearRatios.Length)
        {
            Debug.LogWarning("Invalid gear index");
            return;
        }
        
        if (!Engine.isEngineRunning || !IsValidGearChange(gearIndex)) return;
        
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
        if (isGrinding) StopGrindingSound();
        currentGearIndex = gearIndex;
        //attemptedGearIndex = -1;
        dashController.SetGearMessage(GearLabel(currentGearIndex));
        Debug.Log($"Gear changed to: {GearLabel(currentGearIndex)}");
    }
    
    private bool IsValidGearChange(int newGear)
    {
        float speed = rb.linearVelocity.magnitude * 3.6f;

        if (newGear == 0 || newGear == 1) return true;

        if (Mathf.Abs(newGear - currentGearIndex) > 1) return false;

        if (speed < minSpeedsForGears[newGear])
        {
            Debug.LogWarning($"Too slow to shift into {GearLabel(newGear)} (Speed: {speed:F1} km/h)");
            PlayGrindingSound();
            return false;
        }

        return true;
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

    private void PlayGrindingSound()
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
    }
}
