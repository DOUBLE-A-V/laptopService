using System;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class Tip : MonoBehaviour
{
    public TipDescriptor currentTip;
    [SerializeField] private TMP_Text text;
    [SerializeField] private SpriteRenderer bg;
    private Vector2 GetTextRealSize()
    {
        return new Vector2(text.renderedWidth, text.renderedHeight);
    }
    
    public void ShowTip(TipDescriptor tip, Vector3 pos)
    {
        currentTip = tip;
        text.DOKill();
        text.SetText(tip.CookText());
        text.ForceMeshUpdate(false, true);
        text.color = new Color(1, 1, 1, 0);
        text.DOFade(1, 0.5f);
        Vector2 realSize = GetTextRealSize();
        if (pos.x > 0)
        {
            if (pos.y > 0)
            {
                text.transform.position = pos + new Vector3(-realSize.x/2, -0.5f, 0);
            }
            else
            {
                text.transform.position = pos + new Vector3(-realSize.x/2, realSize.y+0.5f, 0);
            }
        }
        else
        {
            if (pos.y > 0)
            {
                text.transform.position = pos + new Vector3(0.5f, -0.5f, 0);
            }
            else
            {
                text.transform.position = pos + new Vector3(0.5f, realSize.y+0.5f, 0);
            }
        }
        
        //bg.transform.position = text.transform.position;// + new Vector3(realSize.x, -realSize.y, 0)/2;
        bg.DOKill();
        bg.transform.DOKill();
        bg.transform.localScale = Vector2.zero;
        bg.transform.DOScale(new Vector2(realSize.x, realSize.y), 1f).SetEase(Ease.OutExpo);
        bg.transform.DOMove(text.transform.position + new Vector3(realSize.x, -realSize.y)/2, 1f).SetEase(Ease.OutExpo);
        bg.DOFade(1, 0.5f);
    }

    public void HideTip()
    {
        text.DOKill();
        bg.DOKill();
        text.DOFade(0, 0.5f);
        bg.DOFade(0, 0.5f);
    }
}
