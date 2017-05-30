using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectManager : MonoBehaviour {

    [Header("Player")]
    public bool immortalPlayer = false;
    public GameObject Player;
    [Header("Enemies")]
    public GameObject Enemies;    
        
    void Awake () {
        SceneObjectContainer.CreateItemsContainer();
        SceneObjectContainer.CreateProjectileMainContainer();
        SceneObjectContainer.CreateDieEffectsContainer();
        if (Player != null)
        {
            SceneObjectContainer.CreatePlayerContainer();
            GameObject playertemp = Instantiate(Player);
            if (immortalPlayer)
            {
                playertemp.GetComponent<Destroyable>().immortal = true;
            }
        }
        if (Enemies != null)
        {
            SceneObjectContainer.CreateEnemiesContainer();
            Instantiate(Enemies);            
        }        
    }
}
