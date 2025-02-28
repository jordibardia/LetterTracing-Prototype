using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gradingText;

    [SerializeField]
    private TextMeshProUGUI letterText;

    private string[] possibleGraphemes = new string[]{ "S", "C" };

    [SerializeField]
    private List<GameObject> prefabs;

    [SerializeField]
    private List<GameObject> tracingBounds;

    [SerializeField]
    private TracingManager tracingManager;

    private void Start()
    {
        CreateTracing();
    }

    public void UpdateGame(bool gradingResult)
    {
        gradingText.text = gradingResult ? "CORRECT" : "INCORRECT";
        //gradingText.color = gradingResult ? new Color32(0, 255, 0, 255) : new Color32(255, 0, 0, 255);
        Destroy(GameObject.FindGameObjectWithTag("TracingNodes"));
        Destroy(GameObject.FindGameObjectWithTag("TracingBounds"));

        CreateTracing();
    }

    private void CreateTracing()
    {
        System.Random selector = new System.Random();
        int selectionInd = selector.Next(0, possibleGraphemes.Length);

        letterText.text = possibleGraphemes[selectionInd];
        Instantiate(prefabs[selectionInd]);
        Instantiate(tracingBounds[selectionInd]);

        tracingManager.UpdateTracingNodes();
    }
}
