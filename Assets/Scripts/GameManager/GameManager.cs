// Unity Framework
using UnityEngine;
using UnityEngine.SceneManagement;

// FrameLord 
using FrameLord.Core;
using FrameLord.EventDispatcher;

public class GameManager : MonoBehaviorSingleton<GameManager>
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void NotifyPlayerDeath()
    {
        GameEventDispatcher.Instance.Dispatch(this, new EvnPlayerDied());
    }
}