using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static NetSystem.Client;
public class FadeWithoutServer : MonoBehaviour
{
    private ClientManager cm;
    private Image image;
    private MenuElement menuElm; 
    void Start()
    {
        cm = FindObjectOfType<ClientManager>();
        image = GetComponent<Image>();
        menuElm = GetComponent<MenuElement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cm.client.Status == ClientStatus.Connected)
        {
            image.color = Color.white;
            menuElm.turnedOn = true;
        }
        else 
        {
            image.color = Color.grey;
            menuElm.turnedOn = false;
        }
    }
}
