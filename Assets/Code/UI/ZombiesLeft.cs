using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZombiesLeft : MonoBehaviour
{
    public ZombieWaves waveSystem;

    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"Wave:{waveSystem.curWave}\nZombies Left:{waveSystem.zombieStateSystem.zombies.Count}";
    }
}
