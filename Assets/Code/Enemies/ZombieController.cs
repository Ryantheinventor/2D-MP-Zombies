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
    public State myState = State.Waiting;

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
                break;
            case State.Tracking:
                MoveTo(myTarget.transform.position);
                break;
            defaultZ:
                break;



        }



    }

    private bool TargetVisible() 
    {
        int layerMask = 1 << 7;
        //Debug.Log($"After:  {Convert.ToString(layerMask, toBase: 2)}");
        Vector3 direction = myTarget.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, ~layerMask);
        
        Debug.Log(hit.transform.name);
        TargetIsVisible = hit.transform.tag == "Player";

        if (TargetIsVisible)
        {
            Debug.DrawRay(transform.position, direction, Color.green, TargetCheckTime);
        }
        else
        {
            Debug.DrawRay(transform.position, direction, Color.red, TargetCheckTime);
        }

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




}
