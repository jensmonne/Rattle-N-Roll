using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;

    public int zombieskilled;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Addscore()
    {
        zombieskilled++;
    }


}
