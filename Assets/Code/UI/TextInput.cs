using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextInput : MenuElement
{

   
    public int charLimit = 16;
    public string text = "";

    private float curCursorTime = 0f;
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = transform.GetComponentInChildren<TextMeshProUGUI>();
    }


    private void Update() 
    {
        if (curCursorTime < 1f && focused)
        {
            textMesh.text = text + "|";
        }
        else if (curCursorTime > 1f && focused)
        {
            textMesh.text = text;
            if (curCursorTime > 2f)
            {
                curCursorTime = 0;
            }
        }
        else
        {
            textMesh.text = text;
        }
        curCursorTime += Time.deltaTime;

        if (focused) 
        {
            //loop yoinked straight from unity docs 10/10 would yoink again
            foreach (char c in Input.inputString)
            {
                if (c == '\b') // has backspace/delete been pressed?
                {
                    if (text.Length != 0)
                    {
                        text = text.Substring(0, text.Length - 1);
                    }
                }
                else if ((c == '\n') || (c == '\r')) // enter/return
                {
                    focused = false;
                }
                else
                {
                    if (text.Length < charLimit) 
                    {
                        text += c;
                    }
                }
            }
        }
        


    }

    

}
