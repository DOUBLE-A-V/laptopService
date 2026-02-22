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
    
    [SerializeField] private SpriteRenderer spriteRenderer;

    public int level;

    public float costFrom = 0;
    public float costTo = 0;

    public float cost = 0;
    
    protected override void Interact()
    {
        transform.DOKill();
        transform.localScale = Vector3.one * 0.9f;
        transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
        if (Main.obj.money >= cost) Buy();
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
        costText.DOFade(0, 0.5f);
        spriteRenderer.transform.DOKill();
        spriteRenderer.transform.DOScale(new Vector3(0, 0.4f, 1), 1).SetEase(Ease.OutExpo);
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
        spriteRenderer.sprite = toolBase.spriteRenderer.sprite;
        spriteRenderer.transform.DOKill();
        spriteRenderer.transform.DOScale(toolBase.spriteRenderer.transform.localScale, 0.3f).SetEase(Ease.OutExpo);
        costText.DOFade(1, 0.5f);
        costText.text = cost + "$";
        active = true;
        inShop = true;
        transform.position = itemPlace.position;
        transform.DOKill();
        transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
    }
}
