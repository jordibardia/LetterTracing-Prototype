using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public Camera m_camera;
    public GameObject brush;
    public GameObject boundingBox;
    public GameObject boardForeground;

    List<LineRenderer> strokes;
    List<LineRenderer> boundingBoxes;
    LineRenderer currentLineRenderer;

    Vector2 lastPos;

    private void Start()
    {
        strokes = new List<LineRenderer>();
        boundingBoxes = new List<LineRenderer>();
    }

    private void Update()
    {
        Drawing();
    }

    void Drawing()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateBrush();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            PointToMousePos();
        }
        else if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse up");

            if (currentLineRenderer != null)
            {
                strokes.Add(currentLineRenderer);
                DrawBoundingBox(currentLineRenderer);

                currentLineRenderer = null;
            }
        }
    }

    void CreateBrush()
    {
        if (!IsMouseInBoard())
            return;

        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);

        GameObject brushInstance = Instantiate(brush);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);

    }

    void AddAPoint(Vector2 pointPos)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    void PointToMousePos()
    {
        if (!IsMouseInBoard())
            return;

        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);

        if (lastPos != mousePos)
        {
            AddAPoint(mousePos);
            lastPos = mousePos;
        }
    }

    private bool IsMouseInBoard()
    {
        Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
        var rayHit = Physics2D.GetRayIntersection(ray);

        if (rayHit.collider == null)
            return false;

        return rayHit.collider.gameObject == boardForeground;
    }

    private void DrawBoundingBox(LineRenderer stroke)
    {
        Bounds bounds = stroke.bounds;

        Vector3[] boundingBoxPoints = {
            new Vector3(bounds.min.x, bounds.min.y, bounds.center.z),
            new Vector3(bounds.min.x, bounds.max.y, bounds.center.z),
            new Vector3(bounds.max.x, bounds.max.y, bounds.center.z),
            new Vector3(bounds.max.x, bounds.min.y, bounds.center.z),
            new Vector3(bounds.min.x, bounds.min.y, bounds.center.z)
        };

        GameObject boundingBoxInstance = Instantiate(boundingBox);
        LineRenderer boundingBoxLineRenderer = boundingBoxInstance.GetComponent<LineRenderer>();
        boundingBoxLineRenderer.positionCount = boundingBoxPoints.Length;
        boundingBoxLineRenderer.SetPositions(boundingBoxPoints);
        boundingBoxes.Add(boundingBoxLineRenderer);
    }
}