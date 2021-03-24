using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static NetSystem.Client;
public class ClientStatusText : MonoBehaviour
{
    ClientManager cm;
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        cm = FindObjectOfType<ClientManager>();
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (cm.client.Status) 
        {
            case ClientStatus.Connecting:
                text.text = $"Connecting to server...";
                text.color = Color.white;
                break;
            case ClientStatus.Connected:
                text.text = $"Connected:{cm.client.Ping}";
                text.color = Color.green;
                break;
            case ClientStatus.Failed:
                text.text = "Could not find server.";
                text.color = Color.red;
                break;
            case ClientStatus.Disconnected:
                text.text = "Disconnected from server.";
                text.color = Color.red;
                break;
        }
    }
}
