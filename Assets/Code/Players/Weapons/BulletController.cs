using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int damage = 1;
    public float speed = 0.9f;
    // Update is called once per frame
    void Update()
    {
        //use a raycast to check if anything gets in the way of the bullet while moving
        Vector3 direction = new Vector3(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad)) * speed * Time.deltaTime;
        int layerMask = 11 << 7;//obstruction and zombies only
        Physics2D.queriesHitTriggers = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector3.Distance(Vector3.zero,direction), layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Enemy")) 
            {
                //if there is a zombie in the way make it take damage
                hit.collider.gameObject.GetComponent<ZombieController>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else 
        {
            transform.position += direction;
        }
        Physics2D.queriesHitTriggers = true;
    }
}
