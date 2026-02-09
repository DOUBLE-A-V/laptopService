using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class Shop : Place
{
    public List<ShopItem> shopItems1;
    public List<ShopItem> shopItems2;
    public List<ShopItem> shopItems3;

    public List<Transform> itemsPlaces1;
    public List<Transform> itemsPlaces2;
    public List<Transform> itemsPlaces3;

    public GameObject curtain;
    private Vector3 defCurtainPos;
    
    [SerializeField] private UpgradeShopButton upgradeShopButton;

    public int level = 0;
    
    private bool firstEnter = true;


    private void Awake()
    {
        defCurtainPos = curtain.transform.position;
        Main.interactables.Add(upgradeShopButton);
    }
    
    public override void OnEnter()
    {
        if (firstEnter)
        {
            firstEnter = false;
            UpdateAllItems();
        }
        upgradeShopButton.active = level != 2;
        curtain.transform.position = defCurtainPos;
        ShowItems();
        curtain.transform.DOMove(defCurtainPos + new Vector3(0, 2.5f * (level + 1), 0), 1.5f).SetEase(Ease.OutExpo);
        upgradeShopButton.transform.DOKill();
        if (upgradeShopButton.defPos == Vector3.zero)
        {
            upgradeShopButton.defPos = upgradeShopButton.transform.position;
        }
        upgradeShopButton.transform.position = upgradeShopButton.defPos;
        upgradeShopButton.transform.DOMove(upgradeShopButton.defPos + new Vector3(0, -1.5f * level, 0), 1.5f).SetEase(Ease.OutExpo);
        
    }

    public override void OnExit()
    {
        upgradeShopButton.active = false;
    }

    public void Upgrade()
    {
        level++;
        curtain.transform.DOMove(defCurtainPos + new Vector3(0, 2.5f * (level + 1), 0), 1.5f).SetEase(Ease.OutExpo);
        //UpdateItems(level);
        //ShowItems();
    }

    public void ShowItems()
    {
        foreach (ShopItem shopItem in shopItems1) if (shopItem.inShop) shopItem.Show();

        if (level > 0) foreach (ShopItem shopItem in shopItems2) shopItem.Show();
        if (level > 1) foreach (ShopItem shopItem in shopItems3) shopItem.Show();
    }

    public void UpdateAllItems()
    {
        for (int i = 0; i < level+1; i++) UpdateItems(i);
    }

    public void UpdateItems(int forLevel = 0)
    {
        List<ShopItem> prevItems = new List<ShopItem>();
        List<ShopItem> shopItems;
        List<Transform> itemsPlaces;
        if (forLevel == 0)
        {
            shopItems = shopItems1;
            itemsPlaces = itemsPlaces1;
        }
        else if  (forLevel == 1)
        {
            shopItems = shopItems2;
            itemsPlaces = itemsPlaces2;
        }
        else
        {
            shopItems = shopItems3;
            itemsPlaces = itemsPlaces3;
        }
        foreach (ShopItem shopItem in shopItems)
        {
            prevItems.Add(shopItem);
            shopItem.RemoveFromShop();
        }

        foreach (Transform itemPlace in itemsPlaces)
        {
            if (shopItems.Count == 0) break;
            ShopItem item = shopItems[Random.Range(0, shopItems.Count)];
            shopItems.Remove(item);
            item.itemPlace = itemPlace;
            item.inShop = true;
        }

        foreach (ShopItem shopItem in prevItems) shopItems.Add(shopItem);
        prevItems.Clear();
    }
}
