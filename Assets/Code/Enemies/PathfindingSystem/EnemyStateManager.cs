using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{

    [SerializeField]
    protected List<ZombieController> zombies = new List<ZombieController>();


    void Update()
    {
        foreach (ZombieController z in zombies) 
        {
            switch (z.myState) 
            {
                case ZombieController.State.Waiting:
                    if (z.TargetIsVisible) 
                    {
                        z.myState = ZombieController.State.Tracking;
                    }
                    break;
                case ZombieController.State.Following:
                    if (z.TargetIsVisible)
                    {
                        z.myState = ZombieController.State.Tracking;
                    }
                    break;
                case ZombieController.State.Leading:
                    if (z.TargetIsVisible)
                    {
                        z.myState = ZombieController.State.Tracking;
                    }
                    break;
                case ZombieController.State.Tracking:
                    if (!z.TargetIsVisible)
                    {
                        z.myState = ZombieController.State.Waiting;
                    }
                    break;
                default:
                    z.myState = ZombieController.State.Waiting;
                    break;

            }
        }
    }


    public void NewZombie(GameObject zom) 
    {
        zombies.Add(zom.GetComponent<ZombieController>());
    }
}
