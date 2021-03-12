using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class TempGameEnd : MonoBehaviour
{

    //this code will be replaced as the networking system starts getting implemented

    public TextMeshProUGUI text;
    PlayerController playerController;
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        text.enabled = false;
    }

    
    void Update()
    {
        if(playerController.health <= 0)
        {
            playerController.enabled = false;
            text.enabled = true;
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                SceneManager.LoadScene(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Application.Quit();
        }
    }
}
