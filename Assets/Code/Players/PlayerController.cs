using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 moveV = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
        moveV.Normalize();
        transform.position += moveV * speed * Time.deltaTime;
    }
}
