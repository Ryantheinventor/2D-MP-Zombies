using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetSystem;
using static NetSystem.Client;
public class ClientManager : MonoBehaviour
{
    public Client client;
    // Start is called before the first frame update
    void Start()
    {
        client = new Client(new List<string> { "10.0.0.4" }, 7777);
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(client.Status);
        client.CallData();
        client.CallEvents();
    }

    void OnApplicationQuit()
    {
        if (client.Status == ClientStatus.Connected) 
        {
            client.SendPacket(new DataPacket { isEvent = true, varName = "serverDisconnect" });
            client.Stop();
        }
        
    }

}
