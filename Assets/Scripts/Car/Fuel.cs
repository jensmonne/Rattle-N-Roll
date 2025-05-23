using UnityEngine;
using UnityEngine.SceneManagement;

public class Fuel : MonoBehaviour
{
     public static float maxfuelAmount = 1000f;
     public static float currentFuelAmount;
     [SerializeField] private Rigidbody rb;

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

    private void LateUpdate()
    {
        if (rb.linearVelocity.magnitude * 3.6f <= 0.001 && currentFuelAmount <= 0)
        {
            SceneManager.LoadScene("Scenes/Shop");
        }
    }
}
