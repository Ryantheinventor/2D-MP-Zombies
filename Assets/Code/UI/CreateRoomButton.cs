using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetSystem;
using UnityEngine.SceneManagement;
public class CreateRoomButton : MenuElement
{

    public override void ClickOn()
    {
        base.ClickOn();
        if (turnedOn) 
        {
            ClientManager.Instance.client.SendPacket(new DataPacket() { isEvent = true, varName = "newRoom" });
            SceneManager.LoadScene(1);
        }
        
    }

}
