using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class TempGameEnd : MonoBehaviour
{
    public TextMeshProUGUI text;
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        text.enabled = false;
    }

    // Update is called once per frame
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
    }
}
