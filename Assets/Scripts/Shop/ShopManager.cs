namespace Shop
{
 using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<ShopItemConfig> _shopItems;

    private int _coins;

    public void UpdateManager()
    {
        LoadCoins();
        InitShop();
    }
    private void LoadCoins()
    {
        _coins = PlayerPrefs.GetInt("Coins", 0);
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt("Coins", _coins);
    }

    private void InitShop()
    {
        foreach (var item in _shopItems)
        {
            bool isBought = IsItemBought(item.id);

            item.image.sprite = item.sprite;
            item.priceText.text = $"{item.price}";

            if (isBought)
            {
                SetBoughtState(item);
            }
            else
            {
                SetAvailableState(item);
            }
        }
    }

    private void SetBoughtState(ShopItemConfig item)
    {
        item.buyButton.interactable = false;
        item.buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "BOUGHT";
    }

    private void SetAvailableState(ShopItemConfig item)
    {
        item.buyButton.interactable = true;
        item.buyButton.onClick.RemoveAllListeners();
        item.buyButton.onClick.AddListener(() => BuyItem(item));
        item.buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "BUY";
    }

    private void BuyItem(ShopItemConfig item)
    {
        if (_coins < item.price) return;

        _coins -= item.price;
        SaveCoins();
        SavePurchase(item.id);

        SetBoughtState(item);
    }

    private void SavePurchase(string itemId)
    {
        PlayerPrefs.SetInt($"ShopItem_{itemId}", 1);
    }

    private bool IsItemBought(string itemId)
    {
        return PlayerPrefs.GetInt($"ShopItem_{itemId}", 0) == 1;
    }
    
    public void AddCoins(int amount)
    {
        _coins += amount;
        SaveCoins();
    }
    public Sprite GetSpriteById(string id)
    {
        foreach (var item in _shopItems)
        {
            if (item.id == id)
                return item.sprite;
        }
        return null;
    }

}
}
