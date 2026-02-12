using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CompressedAitCanisterTool : Tool
{
    [SerializeField] private BoxCollider2D collision;


    protected override void OnUp()
    {
        collision.gameObject.SetActive(true);
    }

    protected override void OnDown()
    {
        collision.gameObject.SetActive(false);
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
        if (against.targetName == "dust" || against.targetName == "water") against.stackedDamage *= 2;
    }

    public override void CheckCollision()
    {
        List<Collider2D> cols = new List<Collider2D>();
        collision.Overlap(cols);
        foreach (LaptopTarget target in Main.obj.workplace.currentLaptop.targets)
        {
            bool touches = false;
            foreach (Collider2D col in cols)
            {
                if (col.gameObject == target.gameObject)
                {
                    touches = true;
                    break;
                }
            }
            if (touches)
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
