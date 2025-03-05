using UnityEngine;

public class TracingNode : MonoBehaviour
{
    [SerializeField]
    public int nodeOrder;

    private void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
