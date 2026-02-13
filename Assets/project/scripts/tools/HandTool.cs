using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandTool : Tool
{
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
}
