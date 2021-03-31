using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPAgent : PlayerController
{
    [HideInInspector]
    public long lastPacket = 0;
    public void UpdatePos(Vector3 posAndRot) 
    {
        transform.position = new Vector3(posAndRot.x, posAndRot.y, 0);
        transform.eulerAngles = new Vector3(0, 0, posAndRot.z);
    }

    public void FixedUpdate()
    {
        //overide base player movement
    }

}
