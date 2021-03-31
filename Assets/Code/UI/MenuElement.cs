using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuElement : MonoBehaviour
{
    public bool focused = false;
    public bool turnedOn = true;
    public virtual void ClickOn()
    {
        foreach (MenuElement me in transform.parent.GetComponentsInChildren<MenuElement>())
        {
            me.Leave();
        }
        focused = true;
    }


    public virtual void Leave()
    {
        focused = false;
    }
}
