using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetSystem;
using static NetSystem.Client;
using UnityEngine.SceneManagement;
public class ClientManager : MonoBehaviour
{
    public Client client;
    private static ClientManager instance;

    public static ClientManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else if (instance == this) 
        {
            client.ResetEvents("RoomHost", "RoomJoin");
            Debug.Log("2");
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }



    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("1");
        UnitySystemConsoleRedirector.Redirect();
        client = new Client(new List<string> { "10.0.0.4" }, 7777);
        client.AddEvent("RoomHost", OnRoomHost);
        client.AddEvent("RoomJoin", OnGameJoin);
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

    
    void OnGameJoin(ClientEvent ce)
    {
        Debug.Log("Join");
        SceneManager.LoadScene(1);
    }
    void OnGameJoinFail(ClientEvent ce)
    {
        Debug.Log("Join Fail");
    }
    void OnRoomHost(ClientEvent ce)
    {
        Debug.Log("Host");
    }

    public void ResetEvents()
    {
        client.ResetEvents("RoomHost", "RoomJoin");
    }


}
