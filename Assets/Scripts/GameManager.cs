using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance;

    public int zombieskilled;

    public static int Money = 20;

    private void Awake()
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

    public void GameOver()
    {
        Debug.Log("Game Over");
        Money += zombieskilled/2;
    }


}
