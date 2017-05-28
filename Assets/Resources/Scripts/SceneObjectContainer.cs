using System.Collections.Generic;
using UnityEngine;

public static class SceneObjectContainer {

    public static GameObject Player;
    public static GameObject Enemies;
    public static GameObject DieEffect;
    public static GameObject Items;
    public static GameObject ProjectilesMain;
    public static List<ProjectilesCollection> Projectiles = new List<ProjectilesCollection>();
    public static GameObject Projectiles_current;
    public static GameObject Projectiles_gun_current;

    public static void CreatePlayerContainer()
    {
        Player = new GameObject("PlayerContainer");
    }
    public static void CreateEnemiesContainer()
    {
        Enemies = new GameObject("EnemiesContainer");
    }
    public static void CreateDieEffectsContainer()
    {
        DieEffect = new GameObject("DieEffects");
    }
    public static void CreateProjectileMainContainer()
    {
        ProjectilesMain = new GameObject("ProjectilesContainer");
    }
    public static void CreateItemsContainer()
    {
        Items = new GameObject("Items");
    }
    public static void AddProjectileContainer(string name)
    {
        if (ProjectilesMain != null)
        {
            ProjectilesCollection temp = new ProjectilesCollection();
            temp.Name = new GameObject(name);
            temp.Name.transform.parent = ProjectilesMain.transform;
            Projectiles_current = temp.Name;
            Projectiles.Add(temp);
        }
    }
    public static void AddGunContainer(string name)
    {
        if (Projectiles_current != null)
        {
            GameObject temp = new GameObject(name);
            temp.transform.parent = Projectiles_current.transform;
            Projectiles_gun_current = temp;
        }
    }
    public static int EnemiesContainer_ActiveChildCount()
    {
        int answer = 0;
        Transform enemiesTransform = Enemies.transform;
        for (int i = 0; i < enemiesTransform.childCount; i++)
        {
            if (enemiesTransform.GetChild(i).gameObject.activeInHierarchy == true)
            {
                answer++;
            }
        }
        return answer;
    }
}

public class ProjectilesCollection
{
    public GameObject Name;
}