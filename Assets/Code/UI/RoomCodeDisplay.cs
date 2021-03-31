using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RoomCodeDisplay : MonoBehaviour
{
    private void Update()
    {
        GetComponent<TextMeshProUGUI>().text = ClientManager.Instance.client.GameID.ToUpper();
    }

}
