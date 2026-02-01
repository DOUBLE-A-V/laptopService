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

    public int level = 0;


    private void Awake()
    {
        defCurtainPos = curtain.transform.position;
    }
    
    public override void OnEnter()
    {
        curtain.transform.position = defCurtainPos;
        ShowItems();
        curtain.transform.DOMove(defCurtainPos + new Vector3(0, 2.5f * (level + 1), 0), 1f).SetEase(Ease.OutElastic, 0.4f);
    }

    public override void OnExit()
    {
        
    }

    public void Upgrade()
    {
        level++;
        curtain.transform.DOMove(defCurtainPos + new Vector3(0, 2.5f * (level + 1), 0), 1f).SetEase(Ease.OutElastic, 0.4f);
        UpdateItems(level);
        ShowItems();
    }

    public void ShowItems()
    {
        foreach (ShopItem shopItem in shopItems1) shopItem.Show();

        if (level > 0) foreach (ShopItem shopItem in shopItems2) shopItem.Show();
        if (level > 1) foreach (ShopItem shopItem in shopItems3) shopItem.Show();
    }

    public void UpdateAllItems()
    {
        for (int i = 0; i < level; i++) UpdateItems(i);
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
            ShopItem item = shopItems[Random.Range(0, shopItems.Count)];
            shopItems.Remove(item);
            item.itemPlace = itemPlace;
        }

        foreach (ShopItem shopItem in prevItems) shopItems.Add(shopItem);
        prevItems.Clear();
    }
}
