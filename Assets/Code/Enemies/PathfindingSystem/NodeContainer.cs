using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeContainer : MonoBehaviour
{
    public List<Node> nodes = new List<Node>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Node n in transform.GetComponentsInChildren<Node>()) 
        {
            nodes.Add(n);
        }
    }
}
