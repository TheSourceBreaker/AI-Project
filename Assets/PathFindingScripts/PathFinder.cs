using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    Wanderer wanderer;//---------------------------------------------------------------------
    List<Node> OpenList; // This references an already existing List of type Node
    List<Node> CloseList;
    public List<Node> routeBack = new List<Node>();

    public static List<Node> allNodes = new List<Node>();

    public Vector3 target;
    public Vector3 startPos;

    public float speed;
    public float radius = .05f;
    public int routeIndex = 0;


    public void Path(Node Begin, Node End)
    {
        OpenList = new List<Node>();
        CloseList = new List<Node>();

        Node currentNode = Begin;
        OpenList.Add(currentNode);

        while (currentNode != End) // fscore and sorting
        {
            currentNode = OpenList[0]; // current node is equal to the node at the start of OpenList (for each Node)

            for (int i = 1; i < OpenList.Count; i++) // This loop sorts and finds the lowest fScore inside the list and equals it to the currNode
            {
                if (OpenList[i].fScore < currentNode.fScore) // Reads through the node inside the list and compares its gScores with the currentNode's gscore
                {
                    currentNode = OpenList[i]; // If true, then current node is = to the specific list spot
                }
            }
            

            OpenList.Remove(currentNode);
            CloseList.Add(currentNode); // Add currentNode to ClosedList
            
            for (int i = 0; i < currentNode.neighbors.Count; i++) // This loop calculates the neighbor's gScore and hScore using Distance
            {
                if(!CloseList.Contains(currentNode.neighbors[i]))
                {
                    currentNode.neighbors[i].gScore = currentNode.gScore + (currentNode.neighbors[i].difficulty * Vector3.Distance(currentNode.transform.position, currentNode.neighbors[i].transform.position));//gScore
                    currentNode.neighbors[i].hScore = Vector3.Distance(currentNode.transform.position, End.transform.position);//hScore

                    currentNode.neighbors[i].prevNode = currentNode; // currentNode is now the previous Node of the neighbor
                    OpenList.Add(currentNode.neighbors[i]);
                }
            }
        }

        while(currentNode != Begin)
        {
            routeBack.Add(currentNode);
            currentNode = currentNode.prevNode;
        }
        routeBack.Reverse();
    }
    

    public void FindNewPath(Vector3 endPos)
    {
        Node startNode = allNodes[0];
        Node endNode = allNodes[0];

        float minStartDist = Mathf.Infinity;
        float minEndDist = Mathf.Infinity;

        foreach (Node node in allNodes) // for each element inside the allNodes 
        {
            float newStartDistance = Vector3.Distance(transform.position, node.transform.position); // checks the min and max of the startPos and endPos
            float newEndDistance = Vector3.Distance(endPos, node.transform.position);

            

            if (newStartDistance < minStartDist) // if 0 is < the min distance/ looks for the distance closest to 0
            {
                startNode = node;
                minStartDist = newStartDistance;
            }

            if(newEndDistance < minEndDist) // if 0 is < the max distance
            {
                endNode = node;
                minEndDist = newEndDistance;
            }
        }
        Path(startNode, endNode);
    }

    private void Start()
    {
        wanderer = GetComponent<Wanderer>();
    }

    void Update()
    {
        if(!wanderer.iSeeEnemy) // The wanderer is inside a bush they wait until the enemy leaves -------------------------------------------------------------
        {
            if(routeIndex < routeBack.Count) // if the player hasn't made it to the end
            {
                transform.position += (routeBack[routeIndex].transform.position - transform.position).normalized * speed * Time.deltaTime; // keep moving towards the end

                if (Vector3.Distance(transform.position, routeBack[routeIndex].transform.position) < radius) // They have gotten into range / made it to a specific node[i]
                {
                    routeIndex++; 
                }
            }
            else if(routeIndex == routeBack.Count) // The agent made it to the end so...
            {
                routeIndex = 0; // clear the index and
                routeBack.Clear(); // clear the listNode endNode = allNodes[0];

                Node endNode = allNodes[0];
                float maxEndDist = 0;

                foreach (Node node in allNodes) // for each element inside the allNodes 
                {
                    float newEndDistance = Vector3.Distance(transform.position, node.transform.position); // calculates the distance between currentPos and node[i] position

                    if (newEndDistance > maxEndDist) // if new distance is greater then current end distance
                    {
                        endNode = node;
                        maxEndDist = newEndDistance;
                    }
                }
                FindNewPath(endNode.transform.position);
            }
        }
    }
}
