using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetSystem;
using UnityEngine.SceneManagement;

public class JoinRoomButton : MenuElement
{

    public TextInput ti;
    public override void ClickOn()
    {
        base.ClickOn();
        if (turnedOn)
        {
            FindObjectOfType<ClientManager>().client.SendPacket(new DataPacket() { isEvent = true, varName = "connectToRoom", strData = ti.text.ToLower()});
            Debug.Log("Join room:" + ti.text.ToLower());
            
        }

    }
}
