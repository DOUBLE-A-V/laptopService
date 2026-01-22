using System.Numerics;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Vector3 = UnityEngine.Vector3;

public class Wallet : Interactable
{
    [SerializeField] private GameObject dynamicPart;

    [SerializeField] private SpriteRenderer staticPart;
    [SerializeField] private SpriteRenderer dynamicPart2;
    
    [SerializeField] private TMP_Text moneyText;

    [SerializeField] private Sprite moneySprite;
    [SerializeField] private Sprite emptySprite;

    private Vector3 defPos;

    private void Awake()
    {
        defPos = transform.position;
        Main.interactables.Add(this);
    }
    protected override void OnHover()
    {
        transform.DOKill();
        dynamicPart.transform.DOKill();
        moneyText.DOKill();
        
        transform.DOMove(defPos + new Vector3(0, 1, 0), 0.5f).SetEase(Ease.OutExpo);
        dynamicPart.transform.DORotate(new Vector3(-179, 0, 0), 1f).SetEase(Ease.OutExpo);
        moneyText.DOColor(Color.white, 0.5f);
        moneyText.text = Main.obj.money + "$";
        if (Main.obj.money < 2)
        {
            staticPart.sprite = emptySprite;
            dynamicPart2.sprite = emptySprite;
        }
        else
        {
            staticPart.sprite = moneySprite;
            dynamicPart2.sprite = moneySprite;
        }
    }

    protected override void OnHoverExit()
    {
        transform.DOKill();
        dynamicPart.transform.DOKill();
        moneyText.DOKill();
        
        transform.DOMove(defPos, 0.5f).SetEase(Ease.OutExpo);
        dynamicPart.transform.DORotate(Vector3.zero, 1f).SetEase(Ease.OutExpo);
        moneyText.text = Main.obj.money + "$";
        moneyText.DOFade(0, 0.5f);
    }
}