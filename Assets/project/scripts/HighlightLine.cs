using UnityEngine;

public class HighlightLine : MonoBehaviour
{
    public GameObject point1;
    public GameObject point2;
    public LineRenderer line;
    public bool used = false;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
    }

    public void Show()
    {
        line.endColor = Color.white;
        line.startColor = Color.white;
    }

    public void Hide()
    {
        line.endColor = new Color(1, 1, 1, 0);
        line.startColor = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        if (!used) return;
        line.positionCount = 2;
        line.SetPosition(0, point1.transform.position);
        line.SetPosition(1, point2.transform.position);
    }
}
