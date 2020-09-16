using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameLord.Pool;

public class EnemyManager : MonoBehaviour
{
    public Transform zombieStartPointUp;
    public Transform zombieStartPointDown;
    public int levels = 10;
    public int zombieAdder = 20;
    public float zombieTimer = 1f;

    private int currentLevel;
    private int zombiesCreated;
    private int currentZombies;
    private int zombiesInLevel;
    private float time = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        currentLevel = 1;
        StartLevel(currentLevel);
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > zombieTimer)
        {
            time = 0;
            if (zombiesCreated < zombiesInLevel)
            {
                SpawnZombie();
                zombiesCreated++;
            }
        }
    }

    private void StartLevel(int level)
    {
        zombiesInLevel = level * zombieAdder;
        currentZombies = zombiesInLevel;
    }

    private void SpawnZombie()
    {
        var zombie = PoolManager.Instance.GetPool("Zombie").GetItem();
        if (zombie != null)
        {
            Zombie createdZombie = zombie.gameObject.GetComponent<Zombie>();
            createdZombie.startPosition = zombieStartPointDown;
            zombie.gameObject.SetActive(true);
        }
    }
}
