using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
{
    private float curFPSWait = 0;
    private int frames = 0;
    // Update is called once per frame
    void Update()
    {
        curFPSWait -= Time.deltaTime;
        frames++;
        if (curFPSWait <= 0) 
        {
            GetComponent<TextMeshProUGUI>().text = "FPS:" + frames;
            curFPSWait = 1;
            frames = 0;
        }
        
    }
}
