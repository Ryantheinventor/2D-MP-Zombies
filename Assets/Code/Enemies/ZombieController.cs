using System.Collections;
using System.Collections.Generic;
using System;
//using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieController : MonoBehaviour
{
    public GameObject myTarget;
    public float speed = 5;
    public float losAngle = 10;
    public float losDist = 4;

    public bool TargetIsVisible = false;
    public float TargetCheckTime = 2f;//seconds between target visibility checks
    private float curCheckTime = 0f;
    public State myState = State.Waiting;

    public GameObject myLeader;
    private GameObject curFollowNode;


    public float nodeOffsetDist = 1;
    public List<GameObject> path = new List<GameObject>();
    private GameObject nodeContainer;
    //[HideInInspector]
    public List<GameObject> nearNodes = new List<GameObject>();

    [HideInInspector]
    public float curWaitTime = 0f;

    private Vector3 facing = new Vector3();
    
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
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        facing = new Vector3(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));
    }


    void Update()
    {
        if (curCheckTime >= TargetCheckTime && myTarget != null) 
        {
            TargetIsVisible = TargetVisible(myTarget);
            curCheckTime = 0;
        }
        curCheckTime += Time.deltaTime;
        switch (myState) 
        {
            case State.Waiting:
                curWaitTime += Time.deltaTime;
                break;
            case State.Following:
                FollowZombie();
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

    public bool TargetVisible(GameObject target)
    {
        int layerMask = 1 << 7;
        //Debug.Log($"After:  {Convert.ToString(layerMask, toBase: 2)}");
        Vector3 direction = target.transform.position - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, ~layerMask);
        return hit.transform.gameObject == target;
    }

    public bool TargetNotVisible(GameObject target)
    {
        int layerMask = 1 << 7;
        //Debug.Log($"After:  {Convert.ToString(layerMask, toBase: 2)}");
        Vector3 direction = target.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, ~layerMask);
        if (hit.transform == null) 
        {
            return false;
        }
        return hit.transform.CompareTag("Obstruction");
    }

    public bool ForceTargetVisibleCheck() 
    {
        TargetIsVisible = TargetVisible(myTarget);
        return TargetIsVisible;
    }

    public bool TargetInLOS(GameObject target)
    {
        if (Vector3.Distance(target.transform.position, transform.position) < losDist)
        {
            Vector3 toTarget = (target.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(toTarget, facing.normalized);
            float difAngle = Mathf.Acos(dot);
            if (difAngle < losAngle * Mathf.Deg2Rad)
            {
                return (TargetVisible(target));
            }
        }
        return false;
    }

    public void MoveTo(Vector3 target) 
    {
        Vector3 moveV = target - transform.position;
        moveV.Normalize();
        facing = moveV;
        transform.position += moveV * speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0,0, Mathf.LerpAngle(transform.eulerAngles.z, Mathf.Atan2(moveV.y, moveV.x) * Mathf.Rad2Deg, 5* Time.deltaTime));
        
    }

    //follow A* path to player
    public void PathToTarget()
    {
        //get a new path if we do not already have one or the end of the current path is no longer where the player is
        if (path.Count == 0)
        {
            path = PathFinder.FindPath(gameObject, myTarget, GameObject.Find("AINodes"));
        }
        else if(!path[path.Count - 1].GetComponent<Node>().TargetIsVisible(myTarget)) 
        {
            path = PathFinder.FindPath(gameObject, myTarget, GameObject.Find("AINodes"));
        }
        //navigate through path nodes
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

    //follow leader zombie
    public void FollowZombie() 
    {
        if (myLeader == null) 
        {
            ChangeState(State.Waiting);
            Debug.LogError("No leading zombie was set");
            return;
        }

        //use same path data as leader
        ZombieController leaderZC = myLeader.GetComponent<ZombieController>();
        if (leaderZC.path.Count > 0) 
        {
            //MoveTo(myLeader.transform.position + (transform.position - myLeader.transform.position).normalized * 2);

            if (curFollowNode != null)
            {

                if (curFollowNode == leaderZC.path[0])
                {
                    curFollowNode = null;
                }
                else if(Vector3.Distance(transform.position, myLeader.transform.position) < 2)
                {
                    //move awway from node if we have arrived before the leader
                    MoveTo(leaderZC.path[0].transform.position 
                        - (myLeader.transform.position - leaderZC.path[0].transform.position).normalized * 2 
                        + (transform.position - leaderZC.path[0].transform.position).normalized * 2);

                }

            }
            else
            {

                MoveTo(leaderZC.path[0].transform.position);

            }
            if (Vector3.Distance(transform.position, leaderZC.path[0].transform.position) < nodeOffsetDist)
            {
                if (leaderZC.path.Count > 1)
                {
                    curFollowNode = leaderZC.path[1];
                }
            }
        }
    }


    //find the navigation node closest to the zombie
    public GameObject FindNearestNode() 
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

    //find the a zombie that is leading within the area of the current node
    public ZombieController FindNearestLeader() 
    {
        if (nearNodes.Count > 0) 
        {
            foreach (ZombieController zombie in nearNodes[0].GetComponent<Node>().nearZombies)
            {
                if (zombie.myState == State.Leading)
                {
                    return zombie;
                }
            }
        }
        return null;
    }


    private void OnDrawGizmos()
    {
        try
        {
            if (myState != State.Waiting)
            {
                Gizmos.color = Color.blue;
                Vector3 curStart = transform.position;
                switch (myState)
                {
                    case State.Leading:
                        for (int i = 0; i < path.Count; i++)
                        {
                            Vector3 curEnd = path[i].transform.position;
                            Gizmos.DrawLine(curStart, curEnd);
                            curStart = curEnd;
                        }
                        Gizmos.DrawLine(curStart, myTarget.transform.position);
                        break;
                    case State.Tracking:
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(curStart, myTarget.transform.position);
                        break;
                    case State.Following:
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawLine(curStart, myLeader.transform.position);
                        //if (path.Count > 0)
                        //{
                        //    Gizmos.color = Color.blue;
                        //    Gizmos.DrawLine(curStart, myLeader.GetComponent<ZombieController>().path[0].transform.position);
                        //}
                        break;
                }
            }
        }
        catch (ArgumentOutOfRangeException e) 
        {
            //weird exception where nothing can be done
        }
        
        
        
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + facing.normalized);
        Gizmos.color = Color.cyan;
        Vector3 a = new Vector3(Mathf.Cos((transform.eulerAngles.z + losAngle) * Mathf.Deg2Rad), Mathf.Sin((transform.eulerAngles.z + losAngle) * Mathf.Deg2Rad));
        Vector3 b = new Vector3(Mathf.Cos((transform.eulerAngles.z - losAngle) * Mathf.Deg2Rad), Mathf.Sin((transform.eulerAngles.z - losAngle) * Mathf.Deg2Rad));
        Gizmos.DrawLine(transform.position, transform.position + a.normalized * losDist);
        Gizmos.DrawLine(transform.position, transform.position + b.normalized * losDist);
    }

    //clean up on state change
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
                myLeader = null;
                break;
        }
        myState = newState;
        switch (myState)
        {
            case State.Leading:
                path = new List<GameObject>();
                break;
            case State.Waiting:
                curWaitTime = 0;
                break;
            case State.Tracking:
                break;
            case State.Following:
                break;
        }

    }



}
