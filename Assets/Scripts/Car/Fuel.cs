using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField] public static float maxfuelAmount = 100f;
    [SerializeField] public static float currentFuelAmount;

    public static void SetCurrentFuelAmount()
    {
        maxfuelAmount = currentFuelAmount;
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
