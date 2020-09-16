// Unity Framework
using UnityEngine;
using UnityEngine.SceneManagement;

// FrameLord 
using FrameLord.Core;
using FrameLord.EventDispatcher;

public class GameManager : MonoBehaviorSingleton<GameManager>
{
    public int initialNumOfLives = 3;

    // Current number of lives
    private int lives;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void Reset()
    {
        lives = initialNumOfLives;
    }

    public void NotifyPlayerDeath()
    {
        lives = Mathf.Max(lives - 1, 0);
        Debug.Log($"Current lives: {lives}");

        GameEventDispatcher.Instance.Dispatch(this, new EvnPlayerDied());
    }

    public bool IsGameOver()
    {
        return lives == 0;
    }

    public int GetLives()
    {
        return lives;
    }

    public void RespawnPlayer()
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().Respawn();
    }
}