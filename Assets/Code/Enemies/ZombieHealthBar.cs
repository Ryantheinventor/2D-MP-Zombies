using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealthBar : MonoBehaviour
{
    public ZombieController zombie;
    private int maxHealth = 0;
    private Transform greenFront;
    private Transform redBack;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = zombie.health;
        greenFront = transform.Find("GreenFront");
        redBack = transform.Find("RedBack");
        transform.parent = null;
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (zombie == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = zombie.transform.position;
        if (zombie.health == maxHealth) 
        {
            redBack.GetComponent<SpriteRenderer>().enabled = false;
            greenFront.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            redBack.GetComponent<SpriteRenderer>().enabled = true;
            greenFront.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (zombie.health > 0)
        {
            float hbSize = (float)zombie.health / (float)maxHealth;
            greenFront.localScale = new Vector3(hbSize, greenFront.localScale.y, greenFront.localScale.z);
            float hbPos = -((1f - hbSize) / 2);
            greenFront.localPosition = new Vector3(hbPos, greenFront.localPosition.y, greenFront.localPosition.z);
        }
        else 
        {
            greenFront.GetComponent<SpriteRenderer>().enabled = false;
        }

        


    }
}
