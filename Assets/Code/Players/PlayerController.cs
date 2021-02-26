using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public List<ZombieController> nearZombies = new List<ZombieController>();
    public bool isFireing = false;
    private void Start()
    {
        FindObjectOfType<EnemyStateManager>().NewPlayer(gameObject);
    }

    private void Update()
    {
        isFireing = Input.GetMouseButton(0);
        
    }

    void FixedUpdate()
    {
        Vector3 moveV = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
        moveV.Normalize();
        transform.position += moveV * speed * Time.deltaTime;
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

}
