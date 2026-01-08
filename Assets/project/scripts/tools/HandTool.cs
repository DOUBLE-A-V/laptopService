using UnityEngine;
using DG.Tweening;

public class HandTool : Tool
{
    protected override void OnBreakTool(LaptopTarget against)
    {
        transform.DOKill();
        transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
        inHand = false;
        Main.obj.workplace.UpdateToolsHand();
    }
}
