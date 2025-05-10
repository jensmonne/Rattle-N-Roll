using TMPro;
using UnityEngine;

public class DashBoardController : MonoBehaviour
{
    [SerializeField] private GameObject HUD;
    [SerializeField] private TMP_Text gearText;
    
    public void SetGearMessage(string currentGear)
    {
        char firstChar = currentGear[0];
        gearText.text = firstChar.ToString();
    }
}
