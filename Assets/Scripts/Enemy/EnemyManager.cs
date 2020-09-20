using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameLord.Pool;
using FrameLord.EventDispatcher;

public class EnemyManager : MonoBehaviour
{
    public Transform zombieStartPointUpRight;
    public Transform zombieStartPointDownRight;
    public Transform zombieStartPointUpLeft;
    public Transform zombieStartPointDownLeft;

    public Transform devilStartPointLeft;
    public Transform devilStartPointRight;

    public Transform juggernautStartPointDown;
    public Transform juggernautStartPointUp;

    public int enemyAdder = 20;
    public float spawnTimer = 1f;
    public float changeLevelTimer = 3f;

    public int devilSpawnRatio = 15;
    public int juggernautSpawnRatio = 2;

    public static int pointsZombie = 10;
    public static int pointsDevil = 150;
    public static int pointsJuggernaut = 500;

    private int currentLevel;
    private int enemiesCreated;
    private static int currentEnemies;
    private static bool isPlayerAlive;
    private int enemiesInLevel;
    private float time = 0f;    

    private State state;

    enum State
    {
        SpawningEnemies,
        PlayingLevel,
        ChangingLevel,
        FinishedGame
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 3;
        StartLevel(currentLevel);
        isPlayerAlive = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.SpawningEnemies:
                if (isPlayerAlive)
                {
                    time += Time.deltaTime;
                    if (time > spawnTimer)
                    {
                        time = 0;
                        if (enemiesCreated < enemiesInLevel)
                        {
                            SpawnEnemy();
                            enemiesCreated++;
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
                    if (currentEnemies <= 0)
                        GameEventDispatcher.Instance.Dispatch(this, new EvnPlayerDied());
                }
                else
                {
                    state = State.FinishedGame;
                }
                break;
            case State.ChangingLevel:
                time += Time.deltaTime;
                if (time > changeLevelTimer)
                {
                    StartLevel(++currentLevel);                        
                    state = State.SpawningEnemies;
                }
                break;
            case State.FinishedGame:
                UnityEngine.Debug.Log("Termino el juego");
                break;
        }
        
    }

    private void StartLevel(int level)
    {
        enemiesInLevel = level * enemyAdder;
        currentEnemies = enemiesInLevel;
        enemiesCreated = 0;
        UnityEngine.Debug.Log("Nivel " + level.ToString());
    }

    private void SpawnEnemy()
    {
        if(enemiesCreated != 0 && enemiesCreated % devilSpawnRatio == 0)
        {
            var devil = PoolManager.Instance.GetPool("Devil").GetItem();
            if (devil != null)
            {
                Devil createdDevil = devil.gameObject.GetComponent<Devil>();
                createdDevil.startPosition = ChangeDevilSpawnPosition();
                devil.gameObject.SetActive(true);
            }
        }
        else if(enemiesCreated != 0 && enemiesCreated % juggernautSpawnRatio == 0)
        {
            var juggernaut = PoolManager.Instance.GetPool("Juggernaut").GetItem();
            if (juggernaut != null)
            {
                Juggernaut createdJuggernaut = juggernaut.gameObject.GetComponent<Juggernaut>();
                createdJuggernaut.startPosition = ChangeJuggernautSpawnPosition();
                juggernaut.gameObject.SetActive(true);
            }
        }
        else
        {
            var zombie = PoolManager.Instance.GetPool("Zombie").GetItem();
            if (zombie != null)
            {
                Zombie createdZombie = zombie.gameObject.GetComponent<Zombie>();
                createdZombie.startPosition = ChangeZombieSpawnPosition();
                zombie.gameObject.SetActive(true);
            }
        }
    }

    private int zombieSpawnPosition = 0;
    private Transform ChangeZombieSpawnPosition()
    {
        zombieSpawnPosition++;
        if (zombieSpawnPosition > 3)
            zombieSpawnPosition = 0;
        switch (zombieSpawnPosition)
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

    private int devilSpawnPosition = 0;
    private Transform ChangeDevilSpawnPosition()
    {
        devilSpawnPosition++;
        if (devilSpawnPosition > 1)
            devilSpawnPosition = 0;
        switch (devilSpawnPosition)
        {
            case 0:
                return devilStartPointRight;
            default:
                return devilStartPointLeft;
        }
    }

    private int juggernautSpawnPosition = 0;
    private Transform ChangeJuggernautSpawnPosition()
    {
        juggernautSpawnPosition++;
        if (juggernautSpawnPosition > 1)
            juggernautSpawnPosition = 0;
        switch (juggernautSpawnPosition)
        {
            case 0:
                return juggernautStartPointUp;
            default:
                return juggernautStartPointDown;

        }
    }

    public static void ZombieDied()
    {
        currentEnemies--;
        GameManager.Instance.NotifyEnemyDeath(pointsZombie);
    }

    public static void DevilDied()
    {
        currentEnemies--;
        GameManager.Instance.NotifyEnemyDeath(pointsDevil);
    }

    public static void JuggernautDied()
    {
        currentEnemies--;
        GameManager.Instance.NotifyEnemyDeath(pointsJuggernaut);
    }
    public static void PlayerDied()
    {
        isPlayerAlive = false;
    }
}
