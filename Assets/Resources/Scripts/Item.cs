using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Heal, DamageUP, HealthUP, SpeedUp, FirerateUp, Upgrade
}

public class Item : AnimatedObject {

    [Header("Effect")]
    public ItemType item_type;
    public bool RandomizeType = false;
    public int value;
    
    void Start () {
        Amination_OnStart();

        if (RandomizeType)
        {
            switch (Random.Range(0, 5))
            {
                case 0:
                    item_type = ItemType.Heal;
                    break;
                case 1:
                    item_type = ItemType.DamageUP;
                    break;
                case 2:
                    item_type = ItemType.HealthUP;
                    break;
                case 3:
                    item_type = ItemType.SpeedUp;
                    break;
                case 4:
                    item_type = ItemType.FirerateUp;
                    break;
                case 5:
                    item_type = ItemType.Upgrade;
                    break;
            }
        }
    }    

    void Update () {
        timerAnimation();
	}    
}
