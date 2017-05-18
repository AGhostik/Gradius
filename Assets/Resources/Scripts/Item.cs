using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Heal, DamageUP, HealthUP, SpeedUp, FirerateUp, Upgrade
}

public class Item : MonoBehaviour {

    public ItemType item_type;
    public bool RandomizeType = false;

    [Header("Animation")]
    public Sprite[] frames = new Sprite[1];

    private int current_frame = 0;
    private float timer_start;
    private float timer;
    private SpriteRenderer render;

    private GameObject tempPlayer; //удалить-заменить!
    private GameObject tempPlayer_gun1;
    private GameObject tempPlayer_gun2;    
    private EventController.dmgUp sendDmg1 = UIDraw.SetGun1Damage;
    private EventController.dmgUp sendDmg2 = UIDraw.SetGun2Damage;

    // Use this for initialization
    void Start () {
        render = gameObject.GetComponent<SpriteRenderer>();

        timer_start = 0.1f;
        timer = timer_start;

        tempPlayer = GameObject.Find("Ship"); //удалить-заменить!
        tempPlayer_gun1 = tempPlayer.transform.GetChild(0).gameObject;
        tempPlayer_gun2 = tempPlayer.transform.GetChild(1).gameObject;

        if (RandomizeType)
        {
            if (Random.Range(0, 1) == 0)
            {
                item_type = ItemType.Heal;
            }
            else
            {
                item_type = ItemType.DamageUP;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject hitObject = col.gameObject;
        if (hitObject.tag == "Player")
        {
            sendMessageItem();
            Destroy(gameObject);
        }
    }

    private void sendMessageItem()
    {
        //изменить в будущем!
        switch (item_type)
        {
            case ItemType.Heal:
                tempPlayer.GetComponent<Destroyable>().Heal(20);
                break;
            case ItemType.DamageUP:
                int dmUp1 = Random.Range(1, 5);
                int dmUp2 = Random.Range(1, 10);
                tempPlayer_gun1.GetComponent<Gun>().damageUP += dmUp1;
                tempPlayer_gun2.GetComponent<Gun>().damageUP += dmUp2;
                sendDmg1(dmUp1);
                sendDmg2(dmUp2);
                break;
        }
    }

    // Update is called once per frame
    void Update () {        
        Anim();
	}

    private void Anim()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (current_frame < frames.Length - 1)
            {
                current_frame++;
            }
            else
            {
                current_frame = 0;
            }
            changeFrame(current_frame);
            timer = timer_start;
        }
    }

    private void changeFrame(int frame_number = 0)
    {
        render.sprite = frames[frame_number];
    }
}
