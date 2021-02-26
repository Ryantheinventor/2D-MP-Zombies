using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : MonoBehaviour
{


    public struct Connection 
    {
        public Node connection;
        public float distance;
    }

    public List<GameObject> connectedNodes = new List<GameObject>();

    public List<Connection> connections = new List<Connection>();




    void Start()
    {
        foreach (GameObject g in connectedNodes) 
        {
            Connection connection = new Connection();
            connection.connection = g.GetComponent<Node>();
            connection.distance = Mathf.Abs((g.transform.position - transform.position).magnitude);
            connections.Add(connection);
        }    
    }

    void Update()
    {
        
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, gameObject.name);
        foreach (GameObject g in connectedNodes)
        {
            if (g.GetComponent<Node>().connectedNodes.Contains(gameObject))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, g.transform.position);
            }
            else 
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position , g.transform.position);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position + (g.transform.position - transform.position ) / 2, g.transform.position);
            }
        }
    }

    public bool TargetIsVisible(GameObject target) 
    {
        int layerMask = 1 << 7;
        //Debug.Log($"After:  {Convert.ToString(layerMask, toBase: 2)}");
        Vector3 direction = target.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, ~layerMask);

        return hit.transform.tag == "Player";
    }

    public float DistanceToTarget(GameObject target) 
    {
        return Vector3.Distance(transform.position,target.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<ZombieController>().nearNodes.Add(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            ZombieController zc = collision.GetComponent<ZombieController>();
            if (zc.nearNodes.Contains(gameObject))
            {
                zc.nearNodes.Remove(gameObject);
            }
        }
    }


}
