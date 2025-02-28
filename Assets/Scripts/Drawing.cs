using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    [SerializeField]
    public bool drawBoundingBox = true;

    [SerializeField]
    private GameObject letter;
    private TracingManager tracingManager;

    [SerializeField]
    private GameObject gameManager;

    public Camera m_camera;
    public GameObject brush;
    public GameObject boundingBox;
    public GameObject boardForeground;

    List<LineRenderer> boundingBoxes;
    LineRenderer currentLineRenderer;

    Vector2 lastPos;

    private void Start()
    {
        boundingBoxes = new List<LineRenderer>();
        tracingManager = letter.GetComponent<TracingManager>();
    }

    private void Update()
    {
        Drawing();
    }

    void Drawing()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            var nodes = GameObject.FindGameObjectWithTag("TracingNodes");

            foreach (Transform child in nodes.transform)
                child.gameObject.GetComponent<Renderer>().enabled = !child.gameObject.GetComponent<Renderer>().enabled;
        }

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
                if (drawBoundingBox)
                    DrawBoundingBox(currentLineRenderer);

                gameManager.GetComponent<GameManager>().UpdateGame(tracingManager.GradeTracing());

                Destroy(currentLineRenderer);
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

        tracingManager.UpdateProgress();
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
        LayerMask layerMask = LayerMask.GetMask("Drawing");
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

        return hit.collider != null;
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