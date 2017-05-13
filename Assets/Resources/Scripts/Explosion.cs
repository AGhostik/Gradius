using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public Sprite[] explosion = new Sprite[3];

    private int current_frame = 0;
    private const float timer_const = 0.05f;
    private float timer = timer_const;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(Time.deltaTime * 4, 0, 0);
        Die();
    }

    private void Die()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (current_frame == explosion.Length - 1)
            {
                Destroy(gameObject);
            }
            changeFrame(current_frame);
            current_frame++;
            timer = timer_const;
        }
    }

    private void changeFrame(int frame_number = 0)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = explosion[frame_number];
    }
}
