using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public enum State 
    {
        Waiting,//waiting for a task (either leader or follower)
        Following,//following(direct move) a leader through the map (checking if a player is directly trackable will switch to tracking)
        Leading,//using A* to navigate map to player's room (checking if a player is directly trackable will switch to tracking)
        Tracking//directly moving to a player
    }



    void Update()
    {
        
    }
}
