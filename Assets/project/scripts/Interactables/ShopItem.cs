using DG.Tweening;
using UnityEngine;
using UnityEngine.Analytics;
using TMPro;

public class ShopItem : Interactable
{
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
    }

    public void Buy()
    {
        Main.obj.money -= cost;
        costText.DOFade(0, 0.5f);
        transform.DOScale(0, 1).SetEase(Ease.OutExpo);
        Main.obj.wallet.Spend(cost);
    }

    public void Show()
    {
        inShop = true;
        transform.position = itemPlace.position;
        transform.DOKill();
        transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
    }
}
