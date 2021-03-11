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
        Vector3 direction = new Vector3(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad)) * speed * Time.deltaTime;
        int layerMask = 11 << 7;
        Physics2D.queriesHitTriggers = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector3.Distance(Vector3.zero,direction), layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Enemy")) 
            {
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


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log(collision.gameObject.tag);
    //    if (collision.gameObject.CompareTag("Enemy"))// || collision.gameObject.CompareTag("Obstruction")) 
    //    {
    //        collision.gameObject.GetComponent<ZombieController>().TakeDamage(damage);
    //        Destroy(gameObject);
    //    }
    //}

}
