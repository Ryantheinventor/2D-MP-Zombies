using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieWaves : MonoBehaviour
{
    public int waveOneHealth = 100;
    public int waveOneCount = 20;
    public int healthPerWave = 10;
    public int zombiesPerWave = 10;


    public float waveBreakTime = 5;
    private float curWaitTime = 0;

    public int curWave = 0;

    public GameObject zombieFab;
    [HideInInspector]
    public EnemyStateManager zombieStateSystem;
    private NodeContainer nodeContainer;
    

    // Start is called before the first frame update
    void Start()
    {
        nodeContainer = FindObjectOfType<NodeContainer>();
        zombieStateSystem = GetComponent<EnemyStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (zombieStateSystem.noZombies) 
        {
            if (curWaitTime >= waveBreakTime)
            {
                for (int i = 0; i < waveOneCount + curWave * zombiesPerWave; i++)
                {
                    GameObject newZombie = Instantiate(zombieFab, transform);
                    newZombie.GetComponent<ZombieController>().health = waveOneHealth + curWave * healthPerWave;
                    int node = 0;
                    bool validSpawnNode = false;
                    while (!validSpawnNode)
                    {
                        validSpawnNode = true;
                        node = Random.Range(0, nodeContainer.nodes.Count);
                        foreach (PlayerController p in zombieStateSystem.players)
                        {
                            if (nodeContainer.nodes[node].TargetIsVisible(p.gameObject))
                            {
                                validSpawnNode = false;
                                break;
                            }
                        }
                    }
                    newZombie.transform.position = nodeContainer.nodes[node].transform.position;

                }
                curWave++;
                curWaitTime = 0;
            }
            else
            {
                curWaitTime += Time.deltaTime;
            }

        }
    }
}
