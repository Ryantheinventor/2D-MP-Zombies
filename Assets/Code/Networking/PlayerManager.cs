using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetSystem;


public class PlayerManager : MonoBehaviour
{
    public GameObject mPAgentFab;
    public List<MPAgent> mPAgents = new List<MPAgent>();

    private ClientManager CM => ClientManager.Instance;
    private void Start()
    { 
        ClientManager.Instance.client.AddEvent("newPlayer", NewPlayer);
        ClientManager.Instance.client.AddEvent("playerLeave", PlayerLeave);
        ClientManager.Instance.client.AddDataProcessor("PlayerPos", PlayerPos);
    }

    public void NewPlayer(ClientEvent e)
    {
        GameObject newP = Instantiate(mPAgentFab);
        mPAgents.Add(newP.GetComponent<MPAgent>());
    }

    public void PlayerLeave(ClientEvent e)
    {
        mPAgents.RemoveAt(mPAgents.Count - 1);
    }
    public void PlayerPos(PacketEvent e) 
    {
        
        int id = e.data.userID;
        Debug.Log(id);
        if (id > CM.client.ClientID) 
        {
            Debug.Log("down1Id");
            id--;
        }
        if (id > mPAgents.Count - 1) 
        {
            NewPlayer(new ClientEvent("fake"));
        }
        mPAgents[id].UpdatePos(new Vector3(e.data.vectorData.x, e.data.vectorData.y, e.data.vectorData.z));
        mPAgents[id].lastPacket = e.data.packetSendTime;
    }
}
