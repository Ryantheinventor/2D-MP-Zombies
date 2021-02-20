using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
        if (cm.client.Connected)
        {
            text.text = $"Connected:{cm.client.Ping}";
            text.color = Color.green;
        }
        else 
        {
            text.text = "Not connected to server";
            text.color = Color.red;
        }
    }
}
