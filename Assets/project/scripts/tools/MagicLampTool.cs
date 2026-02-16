using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MagicLampTool : Tool
{
    public int damageFrom;
    public int damageTo;
    
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
        against.stackedDamage = Random.Range(damageFrom, damageTo);
    }
}
