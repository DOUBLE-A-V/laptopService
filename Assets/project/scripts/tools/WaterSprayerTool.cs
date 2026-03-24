using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaterSprayerTool : Tool
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

    protected override void OnUse(LaptopTarget against)
    {
        if (against.targetName != "dust" && against.targetName != "sticky residue") against.stackedDamage = 0;
        Main.obj.workplace.currentLaptop.targets.Add(Instantiate(Main.obj.workplace.targetsPrefabs.Find(x => x.targetName == "water"), transform.position, Quaternion.identity));
        Main.obj.workplace.currentLaptop.targets[Main.obj.workplace.currentLaptop.targets.Count - 1].transform.parent =
            Main.obj.workplace.transform;
        Main.obj.workplace.currentLaptop.targets[Main.obj.workplace.currentLaptop.targets.Count-1].transform.localScale = Vector3.zero;
        Main.obj.workplace.currentLaptop.targets[Main.obj.workplace.currentLaptop.targets.Count-1].transform.DOScale(Vector3.one * 0.5f, 0.5f).SetEase(Ease.OutExpo);
    }
}
