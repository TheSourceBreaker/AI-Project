using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neighbors = new List<Node>();
    public Node prevNode;
    public float difficulty;
    public float gScore; // Cost
    public float hScore; // Heuristic
    public float fScore
    {
        get
        {
            return gScore + hScore;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < neighbors.Count; i++)
        {
            Gizmos.DrawLine(transform.position, neighbors[i].transform.position);
        }
    }

    void Start()
    {
        PathFinder.allNodes.Add(this);
    }
}
