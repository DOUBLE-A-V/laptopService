using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MarkerRemoverTool : Tool
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
        if (against.targetName != "scribbles")
        {
            if (against.stackedDamage >= against.health)
            {
                LaptopTarget shiny = Instantiate(Main.obj.workplace.targetsPrefabs.Find(x => x.targetName == "shiny"),
                    transform.position, Quaternion.identity);
                Main.obj.workplace.currentLaptop.targets.Add(shiny);
                shiny.transform.localScale = Vector3.zero;
                shiny.transform.parent = Main.obj.workplace.transform;
                shiny.transform.DOScale(Vector3.one * 0.5f, 0.5f).SetEase(Ease.OutExpo);
            }
        }
        else
        {
            against.stackedDamage *= 2;
        }
    }
}
