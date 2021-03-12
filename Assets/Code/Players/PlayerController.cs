using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    public float speed = 10f;
    public List<ZombieController> nearZombies = new List<ZombieController>();
    public bool isFireing = false;
    public float maxDamageTime = 1;
    public float curDamageTime = 0;
    private void Start()
    {
        FindObjectOfType<EnemyStateManager>().NewPlayer(gameObject);
    }

    private void Update()
    {
        isFireing = Input.GetMouseButton(0);
        if (curDamageTime > 0) 
        {
            curDamageTime -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        Vector3 moveV = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
        moveV.Normalize();
        transform.position += moveV * speed * Time.deltaTime;
        Vector3 rMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(rMousePos.y, rMousePos.x) * Mathf.Rad2Deg);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ZombieController zc = collision.GetComponent<ZombieController>();
        if (zc != null) 
        {
            nearZombies.Add(zc);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ZombieController zc = collision.GetComponent<ZombieController>();
        if (zc != null)
        {
            if (nearZombies.Contains(zc)) 
            {
                nearZombies.Remove(zc);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ZombieController zc = collision.gameObject.GetComponent<ZombieController>();
        if (zc != null && curDamageTime <= 0)
        {
            health -= zc.damage;
            curDamageTime = maxDamageTime;
        }
    }

}
