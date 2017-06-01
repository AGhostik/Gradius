using UnityEngine;

public enum ItemType
{
    Heal, DamageUP, HealthUP, SpeedUp, FirerateUp, GunLevelUp, Upgrade
}

public class Item : AnimatedObject {

    [Header("Effect")]
    public int scorePoints = 0;
    public ItemType item_type;
    public bool RandomizeType = false;
    public float value;
    public float value2;
    public GameObject object1;
    public GameObject object2;

    [Header("Move (dont work yet)")]
    [Range(0f, 5f)]
    public float move_speed = 0;

    [Header("TTL")]
    public bool unlimitedTTL = false;
    public float TTL = 15;

    void Start () {
        Amination_OnStart();

        if (RandomizeType)
        {
            switch (Random.Range(0, 6))
            {
                case 0:
                    item_type = ItemType.Heal;
                    value = 50;
                    break;
                case 1:
                    item_type = ItemType.DamageUP;
                    value = 10;
                    value2 = 16;
                    break;
                case 2:
                    item_type = ItemType.HealthUP;
                    value = 20;
                    break;
                case 3:
                    item_type = ItemType.SpeedUp;
                    value = 0.5f;
                    break;
                case 4:
                    item_type = ItemType.FirerateUp;
                    value = 0.05f;
                    value2 = 0.15f;
                    break;
                case 5:
                    item_type = ItemType.Upgrade;
                    break;
            }
        }
    }    

    void Update () {
        timerAnimation();

        if (!unlimitedTTL)
        {
            TTL -= Time.deltaTime;
            if (TTL <= 0)
            {
                Destroy(gameObject);
            }
        }
    }    
}
