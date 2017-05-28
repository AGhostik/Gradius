using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    public GameObject Player;
    public GameObject Enemies;    
        
    void Awake () {
        SceneObjectContainer.CreateItemsContainer();
        SceneObjectContainer.CreateProjectileMainContainer();
        SceneObjectContainer.CreateDieEffectsContainer();
        if (Player != null)
        {
            SceneObjectContainer.CreatePlayerContainer();
            Instantiate(Player);            
        }
        if (Enemies != null)
        {
            SceneObjectContainer.CreateEnemiesContainer();
            Instantiate(Enemies);            
        }        
    }
}
