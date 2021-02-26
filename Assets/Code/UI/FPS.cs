using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
{
    private float curFPSWait = 0;
    // Update is called once per frame
    void Update()
    {
        curFPSWait -= Time.deltaTime;
        if (curFPSWait <= 0) 
        {
            GetComponent<TextMeshProUGUI>().text = "FPS:" + (int)(1 / Time.deltaTime);
            curFPSWait = 1;
        }
        
    }
}
