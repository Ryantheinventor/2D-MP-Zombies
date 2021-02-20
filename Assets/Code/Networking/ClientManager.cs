using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetSystem;
public class ClientManager : MonoBehaviour
{
    public Client client;
    // Start is called before the first frame update
    void Start()
    {
        client = new Client(new List<string> { "10.0.0.4" }, 7777);
        if (client.Connected)
        {
            Debug.Log("Connected to:" + client.client.Client.RemoteEndPoint);
        }
        else 
        {
            Debug.Log("Faild to connect");
        }
    }

    // Update is called once per frame
    void Update()
    {
        client.CallData();
        client.CallEvents();
    }

    void OnApplicationQuit()
    {
        client.SendPacket(new DataPacket { isEvent = true, varName = "serverDisconnect" });
    }

}
