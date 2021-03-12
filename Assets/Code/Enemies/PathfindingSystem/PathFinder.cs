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
        //open/closed list for A*
        List<NodeValues> openList = new List<NodeValues>();
        List<NodeValues> closedList = new List<NodeValues>();

        //create starting node
        NodeValues curNode = new NodeValues();
        ZombieController zc = zombie.GetComponent<ZombieController>();
        curNode.node = zc.FindNearestNode().GetComponent<Node>();
        curNode.fCost = 0;
        curNode.gCost = 0;
        curNode.parentNode = null;
        

        bool nodesLeft = true;
        while (nodesLeft) 
        {
            //add current node to closed list
            closedList.Add(curNode);
            //if the target is visable from the last node then the path is ready to be traced back
            if (curNode.node.TargetIsVisible(target)) 
            {
                return TraceBack(closedList);
            }
            //loop through all connected nodes of cur node
            foreach (Node.Connection c in curNode.node.connections) 
            {
                //if the node has not already been checked yet
                if (!ListContainsNode(c.connection, closedList)) 
                {
                    //if the node has not already been seen yet
                    if (!ListContainsNode(c.connection, openList)) 
                    {
                        //add the node to open list
                        NodeValues cValues = new NodeValues();
                        cValues.node = c.connection;
                        cValues.gCost = c.distance + curNode.gCost;
                        cValues.fCost = cValues.gCost + c.connection.DistanceToTarget(target);
                        
                        cValues.parentNode = curNode;
                        AddNodeValueToList(cValues, openList);
                    }
                }
            }
            //change cur node to lowest cost node in open list
            if (openList.Count > 0)
            {
                curNode = openList[0];
                openList.RemoveAt(0);
            }
            else //no more nodes left to check so give
            {
                nodesLeft = false;
            }
        }
        return new List<GameObject>();
    }

    //trace back through the nodes
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

