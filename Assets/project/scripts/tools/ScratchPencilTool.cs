using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScratchPencilTool : Tool
{
    [SerializeField] private GameObject collisionHighlight;
    [SerializeField] private float colSize;


    protected override void OnUp()
    {
        collisionHighlight.SetActive(true);
    }

    protected override void OnDown()
    {
        collisionHighlight.SetActive(false);
    }
    
    protected override void OnBreakTool(List<LaptopTarget> against)
    {
        active = false;
        transform.DOKill();
        transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
        inHand = false;
        Main.obj.workplace.UpdateToolsHand();

        foreach (LaptopTarget target in against)
        {
            Main.obj.highlightsManager.RemoveLine(target.highlightLineIndex);
            target.highlightLineIndex = -1;
        }
        hitTargets.Clear();
    }

    protected override void OnUse(LaptopTarget against)
    {
        if (against.targetName != "scratches") against.stackedDamage = 0;
    }

    public override void CheckCollision()
    {
        foreach (LaptopTarget target in Main.obj.workplace.currentLaptop.targets)
        {
            if (Vector2.Distance(transform.position, target.transform.position) < target.size/2 + colSize/2)
            {
                if (target.highlightLineIndex == -1)
                {
                    hitTargets.Add(target);
                    target.highlightLineIndex = Main.obj.highlightsManager.AddLine(target.transform.position);
                }
            } else if (target.highlightLineIndex != -1)
            {
                Main.obj.highlightsManager.RemoveLine(target.highlightLineIndex);
                target.highlightLineIndex = -1;
                hitTargets.Remove(target);
            }
        }
    }
}
