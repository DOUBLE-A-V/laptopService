using UnityEngine;
using DG.Tweening;

public class HandTool : Tool
{
    [SerializeField] private GameObject collisionHighlight;
    [SerializeField] private float colSize;
    
    protected override void OnBreakTool(LaptopTarget against)
    {
        transform.DOKill();
        transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
        inHand = false;
        Main.obj.workplace.UpdateToolsHand();
    }

    public override void CheckCollision()
    {
        foreach (LaptopTarget target in Main.obj.workplace.currentLaptop.targets)
        {
            if (Vector2.Distance(transform.localPosition, target.transform.position) < target.size + colSize)
            {
                if (target.highlightLineIndex != -1) target.highlightLineIndex = Main.obj.highlightsManager.AddLine(target.transform.position);
            }
        }
    }
}
