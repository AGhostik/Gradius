using System.Collections.Generic;
using UnityEngine;


public class Spawner : MonoBehaviour {

    public GameObject SpawnPointsCollector;
    public Wave[] Waves;

    private int waveStage;
    private int waveCounter;
    private int waveCounter_old;
    private int activeEnemiesCount;    
    private bool rewardIsActive;
    private float currentPauseTimer;

    private Wave currentWave;
    private Transform groupTransform;
    private List<GameObject> SpawnPoints = new List<GameObject>();

    private int cachedEnemiesCounter = 0;
    private List<GameObject> Enemies_cached = new List<GameObject>();

    private int cachedRewardsCounter = 0;
    private GameObject PreviousWaveReward;
    private List<GameObject> Rewards_cached = new List<GameObject>();

    // Use this for initialization
    void Start () {
        waveStage = 0;
        waveCounter = 0;
        waveCounter_old = -1;
        rewardIsActive = false;
        currentPauseTimer = Waves[0].Pause;
        findSpawnPoints();
        cacheWaves();
    }
	
	// Update is called once per frame
	void Update () {
        if (waveCounter < Waves.Length)
        {
            if (waveCounter != waveCounter_old)
            {
                currentWave = Waves[waveCounter];
                waveCounter_old = waveCounter;
            }

            if (waveStage == 0)
            {
                waitAllKilled();
            }
            if (waveStage == 1)
            {
                waitPickUp();
            }
            if (waveStage == 2)
            {
                waitPause();
            }
            if (waveStage == 3)
            {
                waveStage = 0;
                activateEnemies();
                nextWave();
            }
        }        
	}

    private void waitAllKilled()
    {
        if (currentWave.waitAllKilled)
        {
            activeEnemiesCount = SceneObjectContainer.EnemiesContainer_ActiveChildCount();
            if (activeEnemiesCount == 0)
            {
                waveStage++;
            }
        }
        else
        {
            waveStage++;
        }
    }

    private void waitPickUp()
    {
        if (PreviousWaveReward != null)
        {
            if (!rewardIsActive)
            {
                PreviousWaveReward.SetActive(true);
                rewardIsActive = true;
            }


            if (currentWave.waitPickUp)
            {
                if (!PreviousWaveReward.activeInHierarchy)
                {
                    rewardIsActive = false;
                    waveStage++;
                }
            }
            else
            {
                waveStage++;
            }
        }
        else
        {
            waveStage++;
        }
    }

    private void waitPause()
    {
        if (currentPauseTimer <= 0)
        {
            waveStage++;
        }
        else
        {
            currentPauseTimer -= Time.deltaTime;
        }
    }
    
    private void activateEnemies()
    {        
        foreach (EnemySpawn current_enemySpawn in currentWave.wave)
        {
            if (current_enemySpawn.Enemy != null)
            {
                if (current_enemySpawn.Spawn.Length > 0)
                {
                    foreach (int spawnPointNumber in current_enemySpawn.Spawn)
                    {
                        Enemies_cached[cachedEnemiesCounter].SetActive(true);
                        cachedEnemiesCounter++;
                    }
                }
                else
                {
                    int spawnPointsCount = SpawnPoints.Count;
                    for (int i = 0; i < spawnPointsCount; i++)
                    {
                        Enemies_cached[cachedEnemiesCounter].SetActive(true);
                        cachedEnemiesCounter++;
                    }
                }
            }
        }        
    }

    private void nextWave()
    {        
        waveCounter++;
        if (cachedRewardsCounter < Rewards_cached.Count)
        {
            PreviousWaveReward = Rewards_cached[cachedRewardsCounter];
            cachedRewardsCounter++;
        }
        else
        {
            PreviousWaveReward = null;
        }
        if (waveCounter < Waves.Length)
        {
            currentPauseTimer = Waves[waveCounter].Pause;
        }
    }

    private void findSpawnPoints()
    {
        int childCount = SpawnPointsCollector.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            SpawnPoints.Add(SpawnPointsCollector.transform.GetChild(i).gameObject);
        }
    }

    private void cacheWaves()
    {
        foreach (Wave w in Waves)
        {
            if (w.reward != null)
            {
                GameObject rew = Instantiate(w.reward, SceneObjectContainer.Items.transform);
                rew.SetActive(false);
                Rewards_cached.Add(rew);
            }

            if (SpawnPoints.Count > 0)
            {
                foreach (EnemySpawn enemySpawn in w.wave)
                {
                    if (enemySpawn.Spawn.Length > 0)
                    {
                        foreach (int spawnPointNumber in enemySpawn.Spawn)
                        {
                            if (spawnPointNumber < SpawnPoints.Count)
                            {
                                GameObject enemy = Instantiate(enemySpawn.Enemy, SceneObjectContainer.Enemies.transform);
                                enemy.transform.position = SpawnPoints[spawnPointNumber].transform.position;
                                enemy.SetActive(false);
                                Enemies_cached.Add(enemy);
                            }
                        }
                    }
                    else
                    {
                        int spawnPointsCount = SpawnPoints.Count;
                        for (int i = 0; i < spawnPointsCount; i++)
                        {
                            GameObject enemy = Instantiate(enemySpawn.Enemy, SceneObjectContainer.Enemies.transform);
                            enemy.transform.position = SpawnPoints[i].transform.position;
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
    [Header("Wave options")]
    public bool waitAllKilled = false;
    public float Pause = 0;
    [Header("Reward")]
    public bool waitPickUp = true;
    public GameObject reward;
    [Header("Enemies")]
    public EnemySpawn[] wave;    
}

[System.Serializable]
public class EnemySpawn
{
    public GameObject Enemy;
    public int[] Spawn;
}