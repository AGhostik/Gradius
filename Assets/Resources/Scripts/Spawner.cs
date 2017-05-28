using System.Collections.Generic;
using UnityEngine;


public class Spawner : MonoBehaviour {
    
    public GameObject[] SpawnPoints;
    public Wave[] Waves;    

    private int waveCounter = 0;
    private float currentPauseTimer;
    private Wave currentWave;
    private Transform groupTransform;

    private int cachedEnemiesCounter = 0;
    private List<GameObject> Enemies_cached = new List<GameObject>();

    // Use this for initialization
    void Start () {
        currentPauseTimer = Waves[0].Pause;
        currentWave = Waves[0];
        cacheEnemies();
    }
	
	// Update is called once per frame
	void Update () {
        if (waveCounter < Waves.Length)
        {
            if (currentPauseTimer <= 0)
            {
                if (currentWave.waitAllKilled)
                {
                    int activeEnemiesCount = SceneObjectContainer.EnemiesContainer_ActiveChildCount();
                    if (activeEnemiesCount == 0)
                    {
                        spawn();
                    }
                }
                else
                {
                    spawn();
                }
            }
            else
            {
                currentPauseTimer -= Time.deltaTime;
            }
        }        
	}

    private void spawn()
    {        
        currentWave = Waves[waveCounter];        
        foreach (EnemySpawn current_enemySpawn in currentWave.wave)
        {
            if (current_enemySpawn.Enemy != null)
            {
                foreach (int spawnPointNumber in current_enemySpawn.Spawn)
                {
                    Enemies_cached[cachedEnemiesCounter].SetActive(true);
                    cachedEnemiesCounter++;
                }
            }
        }        
        waveCounter++;
        if (waveCounter < Waves.Length)
        {
            currentPauseTimer = Waves[waveCounter].Pause;
        }        
    }

    private void cacheEnemies()
    {
        if (SpawnPoints.Length > 0)
        {
            foreach (Wave w in Waves)
            {
                foreach (EnemySpawn enemySpawn in w.wave)
                {
                    foreach (int spawnPointNumber in enemySpawn.Spawn)
                    {
                        if (spawnPointNumber < SpawnPoints.Length)
                        {
                            GameObject enemy = Instantiate(enemySpawn.Enemy, SceneObjectContainer.Enemies.transform);
                            enemy.transform.position = SpawnPoints[spawnPointNumber].transform.position;
                            enemy.SetActive(false);
                            Enemies_cached.Add(enemy);
                        }
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class Wave
{
    public bool waitAllKilled = false;
    public float Pause = 0;    
    public EnemySpawn[] wave;    
}

[System.Serializable]
public class EnemySpawn
{
    public GameObject Enemy;
    public int[] Spawn;
}