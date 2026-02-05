using DG.Tweening;
using UnityEngine;
using UnityEngine.Analytics;
using TMPro;

public class ShopItem : Interactable
{
    public Tool toolBase;
    public TMP_Text costText;
    public string itemName;
    public Transform itemPlace;
    public bool inShop = false;

    public int level;

    public float cost = 0;
    
    protected override void Interact()
    {
        transform.DOKill();
        transform.localScale = Vector3.one * 0.9f;
        transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
        Buy();
    }

    public void RemoveFromShop()
    {
        transform.localScale = Vector3.zero;
        inShop = false;
        active = false;
        Main.obj.workplace.tip.HideTip();
    }

    public void Buy()
    {
        Main.obj.workplace.tip.HideTip();
        active = false;
        //costText.DOFade(0, 0.5f);
        transform.DOScale(0, 1).SetEase(Ease.OutExpo);
        Main.obj.wallet.Spend(cost);
        Main.obj.workplace.GiveTool(itemName);
    }
    
    protected override void OnHoverExit()
    {
        Main.obj.workplace.tip.HideTip();
    }

    protected override void OnHover()
    {
        Main.obj.workplace.tip.ShowTip(toolBase.tip, transform.position);
    }

    public void Show()
    {
        costText.text = cost + "$";
        active = true;
        inShop = true;
        transform.position = itemPlace.position;
        transform.DOKill();
        transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
    }
}
