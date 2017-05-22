using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    
    public Wave[] waves;

    private int wave_counter = 0;
    private float current_pauseTimer;
    private Wave current_wave;

    // Use this for initialization
    void Start () {
        current_pauseTimer = waves[0].Pause;
	}
	
	// Update is called once per frame
	void Update () {
        if (wave_counter < waves.Length)
        {
            if (current_pauseTimer <= 0)
            {
                spawn();
            }
            else
            {
                current_pauseTimer -= Time.deltaTime;
            }
        }        
	}

    private void spawn()
    {
        current_wave = waves[wave_counter];        
        foreach (EnemySpawn current_enemySpawn in current_wave.wave)
        {
            if (current_enemySpawn.Enemy != null && current_enemySpawn.GroupObject != null)
            {
                GameObject enemy;
                foreach (GameObject sp in current_enemySpawn.SpawnPoint)
                {
                    if (sp == null)
                    {
                        break;
                    }
                    enemy = Instantiate(current_enemySpawn.Enemy);
                    enemy.transform.position = sp.transform.position;
                    enemy.transform.SetParent(current_enemySpawn.GroupObject.transform);
                }
            }
        }        
        wave_counter++;
        if (wave_counter < waves.Length)
        {
            current_pauseTimer = waves[wave_counter].Pause;
        }
    }
}

[System.Serializable]
public class Wave
{
    public float Pause = 0;
    public EnemySpawn[] wave;    
}

[System.Serializable]
public class EnemySpawn
{
    public GameObject Enemy;
    public GameObject GroupObject;
    public List<GameObject> SpawnPoint;
}