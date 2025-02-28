using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;


public class TracingManager : MonoBehaviour
{
    private List<GameObject> nodes;
    private bool[] visited;
    private List<int> orderVisited;

    private bool exitedPolygon;
    
    void Start()
    {
        var text = GetComponentInChildren<TextMeshProUGUI>();
        text.alpha = 0.5f;
    }

    public void UpdateTracingNodes()
    {
        nodes = new List<GameObject>();
        var tracingNodeObject = GameObject.FindGameObjectWithTag("TracingNodes");

        foreach (Transform child in tracingNodeObject.transform)
            nodes.Add(child.gameObject);

        visited = new bool[nodes.Count];
        orderVisited = new List<int>();
        exitedPolygon = false;
    }

    public void UpdateProgress()
    {
        var nodeLayer = LayerMask.GetMask("TracingNodes");
        var tracingBoundsLayer = LayerMask.GetMask("TracingBounds");

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if tracing bounds has been exited
        var hitBound = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, tracingBoundsLayer);

        if (!exitedPolygon)
        {
            //Debug.Log("In polygon");
            exitedPolygon = (hitBound.collider == null);
        }
        else
        {
            //Debug.Log("Exited polygon");
        }

        // Check if node has been hit
        var hitNode = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, nodeLayer);

        if (hitNode.collider != null)
        {
            var node = hitNode.collider.gameObject.GetComponent<TracingNode>();
            if (node != null)
            {
                if (!visited[node.nodeOrder])
                {
                    Debug.Log($"Node hit: {node.nodeOrder}");
                    visited[node.nodeOrder] = true;
                    orderVisited.Add(node.nodeOrder);
                }
            }
        }
    }

    public bool GradeTracing()
    {
        if (visited.Any(p => !p) || exitedPolygon)
            return false;

        for (int i = 0; i < orderVisited.Count; i++)
        {
            if (orderVisited[i] != i)
                return false;
        }

        return true;
    }



}
