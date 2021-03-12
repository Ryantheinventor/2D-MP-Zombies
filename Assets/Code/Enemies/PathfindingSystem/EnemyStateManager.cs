using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{

    public float maxIdleTime = 10f;

    [SerializeField]
    public readonly List<PlayerController> players = new List<PlayerController>();

    [SerializeField]
    public readonly List<ZombieController> zombies = new List<ZombieController>();

    public bool noZombies { get { return zombies.Count == 0; } }

    void Update()
    {
        foreach (ZombieController z in zombies) 
        {
            switch (z.myState) 
            {
                case ZombieController.State.Waiting:
                    WaitingForTarget(z);
                    break;
                case ZombieController.State.Following:
                    if (z.TargetIsVisible)//if the target is vissible go after target
                    {
                        z.ChangeState(ZombieController.State.Tracking);
                    }
                    else if (z.myLeader != null)//if the leader is still alive
                    {
                        if (z.TargetNotVisible(z.myLeader))//if the leader leaves the follower behind wait for a new job
                        {
                            z.ChangeState(ZombieController.State.Waiting);
                        }
                    }
                    else //if the leader died wait for a new job
                    {
                        z.ChangeState(ZombieController.State.Waiting);
                    }
                    break;
                case ZombieController.State.Leading:
                    if (z.TargetIsVisible)//if the target is vissible go after target
                    {
                        z.ChangeState(ZombieController.State.Tracking);
                    }
                    break;
                case ZombieController.State.Tracking:
                    if (!z.TargetIsVisible)//if the target is no longer vissible wait for a new job
                    {
                        z.ChangeState(ZombieController.State.Waiting);
                    }
                    break;
                default:
                    z.ChangeState(ZombieController.State.Waiting);
                    break;

            }
        }
    }

    public void WaitingForTarget(ZombieController zombie) 
    {
        if (zombie.myTarget == null)
        {
            foreach (PlayerController p in players)
            {
                if (p.isFireing) 
                {
                    zombie.myTarget = p.gameObject;
                    break;
                }
                if (p.nearZombies.Contains(zombie)) 
                {
                    zombie.myTarget = p.gameObject;
                    break;
                }
                if (zombie.TargetInLOS(p.gameObject)) 
                {
                    zombie.myTarget = p.gameObject;
                }
            }
            //if zombie still has no target check if it has reached it's max idle time
            if (zombie.curWaitTime >= maxIdleTime && zombie.myTarget == null)
            {
                zombie.myTarget = players[Random.Range(0,players.Count)].gameObject;
            }
        }

        if (zombie.myTarget != null)
        {
            ZombieController nl = zombie.FindNearestLeader();
            if (nl != null)
            {
                zombie.ChangeState(ZombieController.State.Following);
                zombie.myLeader = nl.gameObject;
            }
            else
            {
                zombie.ChangeState(ZombieController.State.Leading);
            }
        }
         
    }


    public void NewZombie(GameObject zom)
    {
        zombies.Add(zom.GetComponent<ZombieController>());
    }

    public void ZombieKilled(GameObject zom)
    {
        ZombieController zomC = zom.GetComponent<ZombieController>();
        if (zombies.Contains(zomC)) 
        {
            zombies.Remove(zomC);
        }
        
    }

    public void NewPlayer(GameObject player)
    {
        players.Add(player.GetComponent<PlayerController>());
    }

    public void RemovePlayer(GameObject player) 
    {
        PlayerController playerC = player.GetComponent<PlayerController>();
        if (players.Contains(playerC))
        {
            players.Remove(playerC);
        }
    }


}
