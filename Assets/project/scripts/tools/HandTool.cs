using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandTool : Tool
{
    protected override void OnBreakTool(List<LaptopTarget> against)
    {
        Debug.Log("111");
        active = false;
        transform.DOKill();
        Debug.Log("112");
        transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
        Debug.Log("113");
        inHand = false;
        Main.obj.workplace.UpdateToolsHand();
        Debug.Log("114");
        foreach (LaptopTarget target in against)
        {
            Main.obj.highlightsManager.RemoveLine(target.highlightLineIndex);
            target.highlightLineIndex = -1;
        }
        Debug.Log("115");
        hitTargets.Clear();
    }
}
