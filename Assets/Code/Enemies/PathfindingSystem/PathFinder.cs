using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    public class NodeValues 
    {
        public Node node;
        public NodeValues parentNode;
        public float fCost;
        public float gCost;
    }



    public static List<GameObject> FindPath(GameObject zombie, GameObject target, GameObject nodeContainer) 
    {
        ZombieController zc = zombie.GetComponent<ZombieController>();
        List<NodeValues> openList = new List<NodeValues>();
        List<NodeValues> closedList = new List<NodeValues>();
        NodeValues curNode = new NodeValues();
        curNode.node = zc.findNearestNode().GetComponent<Node>();
        curNode.fCost = 0;
        curNode.gCost = 0;
        curNode.parentNode = null;

        bool nodesLeft = true;
        while (nodesLeft) 
        {
            closedList.Add(curNode);
            if (curNode.node.TargetIsVisible(target)) 
            {
                return TraceBack(closedList);
            }
            foreach (Node.Connection c in curNode.node.connections) 
            {
                if (!ListContainsNode(c.connection, closedList)) 
                {
                    if (!ListContainsNode(c.connection, openList)) 
                    {
                        NodeValues cValues = new NodeValues();
                        cValues.node = c.connection;
                        cValues.gCost = c.distance + curNode.gCost;
                        cValues.fCost = cValues.gCost + c.connection.DistanceToTarget(target);
                        
                        cValues.parentNode = curNode;
                        AddNodeValueToList(cValues, openList);
                    }
                }
            }
            if (openList.Count > 0)
            {
                curNode = openList[0];
                openList.RemoveAt(0);
            }
            else 
            {
                nodesLeft = false;
            }
        }
        return TraceBack(closedList);
    }

    private static List<GameObject> TraceBack(List<NodeValues> closedList)
    {
        List<GameObject> path = new List<GameObject>();
        NodeValues curNode = closedList[closedList.Count - 1];
        path.Add(curNode.node.gameObject);
        while (curNode.parentNode != null) 
        {
            path.Insert(0, curNode.parentNode.node.gameObject);
            curNode = curNode.parentNode;
        }
        return path;
    }


    private static void AddNodeValueToList(NodeValues curNode, List<NodeValues> openList) 
    {
        for (int i = 0; i < openList.Count; i++) 
        {
            if (curNode.fCost < openList[i].fCost) 
            {
                openList.Insert(i, curNode);
                return;
            }
        }
        openList.Add(curNode);
    }

    private static bool ListContainsNode(Node node, List<NodeValues> list) 
    {
        foreach (NodeValues n in list) 
        {
            if (n.node == node) 
            {
                return true;
            }
        }
        return false;
    }


}

