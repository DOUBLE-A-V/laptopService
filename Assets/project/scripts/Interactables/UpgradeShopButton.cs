using DG.Tweening;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UpgradeShopButton : Interactable
{
    //[SerializeField] private TMP_Text costText;
    [SerializeField] public List<float> costs;

    public Vector3 defPos;
    
    private void Start()
    {
        UpdateCostText();
        defPos = transform.position;
    }
    
    public void UpdateCostText()
    {
        //if (Main.obj.shop.level != 2) costText.text = "upgrade shop\n" + costs[Main.obj.shop.level] + "$";
        //else costText.DOFade(0, 0.5f).SetEase(Ease.OutExpo);
    }

    protected override void Interact()
    {
        return;
        if (Main.obj.money >= costs[Main.obj.shop.level])
        {
            transform.DOKill();

            transform.DOMove(transform.position - new Vector3(0, 1.5f, 0), 1.5f).SetEase(Ease.OutExpo);
            
            Main.obj.GiveMoney(-costs[Main.obj.shop.level]);
            Main.obj.shop.Upgrade();
            if (Main.obj.shop.level == 2) active = false;
            UpdateCostText();
        } else Main.obj.ShowMessage("not enough money", Main.cam.ScreenToWorldPoint(Input.mousePosition));
    }
}
