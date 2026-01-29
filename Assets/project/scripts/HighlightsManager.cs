using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class HighlightsManager : MonoBehaviour
{
    [SerializeField] private List<HighlightLine> linesPull;

    public int AddLine(Vector3 point2)
    {
        point2.z = 0;
        HighlightLine line = linesPull.Find(x => !x.used);
        line.used = true;
        line.point2.transform.DOKill();
        line.point2.transform.position = Main.cam.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        line.point2.transform.DOMove(point2, 0.5f).SetEase(Ease.OutExpo);
        line.Show();
        return linesPull.IndexOf(line);
    }

    public void RemoveLine(int index)
    {
        HighlightLine line = linesPull[index];
        line.used = false;
        line.Hide();
    }

    private void Update()
    {
        foreach (HighlightLine line in linesPull) if (line.used) line.point1.transform.position = 
            Main.cam.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
    }
}
