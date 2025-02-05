using Unity.VisualScripting;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public Camera m_camera;
    public GameObject brush;
    public GameObject boardForeground;

    LineRenderer currentLineRenderer;

    Vector2 lastPos;

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
        else
        {
            currentLineRenderer = null;
        }
    }

    void CreateBrush()
    {
        if (!isMouseInBoard())
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
        if (!isMouseInBoard())
            return;

        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);

        if (lastPos != mousePos)
        {
            AddAPoint(mousePos);
            lastPos = mousePos;
        }
    }

    private bool isMouseInBoard()
    {
        Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
        var rayHit = Physics2D.GetRayIntersection(ray);

        if (rayHit.collider == null)
            return false;

        return rayHit.collider.gameObject == boardForeground;
    }
}