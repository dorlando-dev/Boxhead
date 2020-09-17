using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameLord.Pool;

public class EnemyManager : MonoBehaviour
{
    public Transform zombieStartPointUpRight;
    public Transform zombieStartPointDownRight;
    public Transform zombieStartPointUpLeft;
    public Transform zombieStartPointDownLeft;
    public int levels = 10;
    public int zombieAdder = 20;
    public float zombieTimer = 1f;
    public float changeLevelTimer = 3f;

    private int currentLevel;
    private int zombiesCreated;
    private static int currentZombies;
    private static bool isPlayerAlive;
    private int zombiesInLevel;
    private float time = 0f;

    private State state;

    enum State
    {
        SpawningZombies,
        PlayingLevel,
        ChangingLevel,
        FinishedGame
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 1;
        StartLevel(currentLevel);
        isPlayerAlive = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.SpawningZombies:
                if (isPlayerAlive)
                {
                    time += Time.deltaTime;
                    if (time > zombieTimer)
                    {
                        time = 0;
                        if (zombiesCreated < zombiesInLevel)
                        {
                            SpawnZombie();
                            zombiesCreated++;
                        }
                        else
                        {
                            state = State.PlayingLevel;
                        }
                    }
                }
                else
                {
                    state = State.FinishedGame;
                }
                break;
            case State.PlayingLevel:
                if (isPlayerAlive)
                {
                    if (currentZombies <= 0)
                        state = State.ChangingLevel;
                }
                else
                {
                    state = State.FinishedGame;
                }
                break;
            case State.ChangingLevel:
                if (currentLevel + 1 <= levels)
                {
                    time += Time.deltaTime;
                    if (time > changeLevelTimer)
                    {
                        StartLevel(++currentLevel);                        
                        state = State.SpawningZombies;
                    }
                }
                else
                {
                    state = State.FinishedGame;
                }
                break;
            case State.FinishedGame:
                UnityEngine.Debug.Log("Termino el juego");
                break;
        }
        
    }

    private void StartLevel(int level)
    {
        zombiesInLevel = level * zombieAdder;
        currentZombies = zombiesInLevel;
        zombiesCreated = 0;
        UnityEngine.Debug.Log("Nivel " + level.ToString());
    }

    private void SpawnZombie()
    {
        var zombie = PoolManager.Instance.GetPool("Zombie").GetItem();
        if (zombie != null)
        {
            Zombie createdZombie = zombie.gameObject.GetComponent<Zombie>();
            createdZombie.startPosition = ChangeSpawnPosition();
            zombie.gameObject.SetActive(true);
        }
    }

    private int spawnPosition = 0;
    private Transform ChangeSpawnPosition()
    {
        spawnPosition++;
        if (spawnPosition > 3)
            spawnPosition = 0;
        switch (spawnPosition)
        {
            case 0:
                return zombieStartPointUpRight;
            case 1:
                return zombieStartPointUpLeft;
            case 2:
                return zombieStartPointDownRight;
            default:
                return zombieStartPointDownLeft;
        }
    }

    public static void ZombieDied()
    {
        currentZombies--;
    }

    public static void PlayerDied()
    {
        isPlayerAlive = false;
    }
}
