using System;
using UnityEngine;

public class Fuel : MonoBehaviour
{
     public static float maxfuelAmount = 100f;
     public static float currentFuelAmount;

    private void Start()
    {
        SetCurrentFuelAmount();
    }

    public static void SetCurrentFuelAmount()
    {
        currentFuelAmount = maxfuelAmount;
    }
    
    public static void AddMaxFuel(float amount)
    {
        maxfuelAmount += amount;
    }

    public static void ResetMaxFuel()
    {
        maxfuelAmount = 100f;
    }
}
