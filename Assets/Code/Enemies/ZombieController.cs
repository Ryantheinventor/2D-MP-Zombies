using System.Collections;
using System.Collections.Generic;
//using System;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public GameObject myTarget;
    public float speed = 5;
    public bool TargetIsVisible = false;
    public float TargetCheckTime = 2f;//seconds between target visibility checks
    private float curCheckTime = 0f;
    private float curPathTime = 0f;
    public State myState = State.Waiting;

    public float nodeOffsetDist = 1;
    public List<GameObject> path = new List<GameObject>();
    private GameObject nodeContainer;
    //[HideInInspector]
    public List<GameObject> nearNodes = new List<GameObject>();
    
    
    public enum State 
    {
        Waiting,//waiting for a task (either leader or follower)
        Following,//following(direct move) a leader through the map (checking if a player is directly trackable will switch to tracking)
        Leading,//using A* to navigate map to player's room (checking if a player is directly trackable will switch to tracking)
        Tracking//directly moving to a player
    }

    private void Start()
    {
        GameObject.FindObjectOfType<EnemyStateManager>().NewZombie(gameObject);
        nodeContainer = GameObject.Find("AINodes");
    }


    void Update()
    {
        if (curCheckTime >= TargetCheckTime) 
        {
            TargetVisible();
            curCheckTime = 0;
        }
        curCheckTime += Time.deltaTime;
        switch (myState) 
        {
            case State.Waiting:
                break;
            case State.Following:
                break;
            case State.Leading:
                PathToTarget();
                break;
            case State.Tracking:
                MoveTo(myTarget.transform.position);
                break;
            default:
                break;
        }



    }

    private bool TargetVisible() 
    {
        int layerMask = 1 << 7;
        //Debug.Log($"After:  {Convert.ToString(layerMask, toBase: 2)}");
        Vector3 direction = myTarget.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, ~layerMask);
        //RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, direction, direction.magnitude, ~layerMask);
        TargetIsVisible = hit.transform.tag == "Player";

        return TargetIsVisible;
    }

    public bool ForceTargetVisibleCheck() 
    {
        TargetVisible();
        return TargetIsVisible;
    }

    public void MoveTo(Vector3 target) 
    {
        Vector3 moveV = target - transform.position;
        moveV.Normalize();
        transform.position += moveV * speed * Time.deltaTime;
    }

    public void PathToTarget()
    {
        if (path.Count == 0)
        {
            path = PathFinder.FindPath(gameObject, myTarget, GameObject.Find("AINodes"));
        }
        else if(!path[path.Count - 1].GetComponent<Node>().TargetIsVisible(myTarget)) 
        {
            path = PathFinder.FindPath(gameObject, myTarget, GameObject.Find("AINodes"));
        }
        if (path.Count > 0) 
        {
            Vector3 targetNodePos = path[0].transform.position;
            MoveTo(targetNodePos);
            if (Vector3.Distance(transform.position, targetNodePos) < nodeOffsetDist) 
            {
                path.RemoveAt(0);
            }
        }
        
    }

    public GameObject findNearestNode() 
    {
        float dist = float.MaxValue;
        GameObject nearestNode = null;
        foreach (GameObject g in nearNodes) 
        {
            float testDist = Vector3.Distance(g.transform.position, transform.position);
            if (testDist < dist) 
            {
                nearestNode = g;
                dist = testDist;
            }
        }
        return nearestNode;
    }


    private void OnDrawGizmos()
    {
        if (myState != State.Waiting)
        {
            Gizmos.color = Color.blue;
            Vector3 curStart = transform.position;
            if (!TargetIsVisible)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    Vector3 curEnd = path[i].transform.position;
                    Gizmos.DrawLine(curStart, curEnd);
                    curStart = curEnd;
                }
                Gizmos.DrawLine(curStart, myTarget.transform.position);
            }
            else 
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(curStart, myTarget.transform.position);
            }
            
        }
    }


    public void ChangeState(State newState) 
    {
        switch (myState) 
        {
            case State.Leading:
                break;
            case State.Waiting:
                break;
            case State.Tracking:
                break;
            case State.Following:
                break;
        }
        myState = newState;
        switch (myState)
        {
            case State.Leading:
                path = new List<GameObject>();
                break;
            case State.Waiting:
                break;
            case State.Tracking:
                break;
            case State.Following:
                break;
        }

    }



}
