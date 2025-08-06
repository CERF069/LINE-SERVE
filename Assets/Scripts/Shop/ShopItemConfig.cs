namespace Shop
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    [System.Serializable]
    public class ShopItemConfig
    {
        public string id;
        public int price;
        public Sprite sprite;
        public SkinType skinType;
        public Image image;
        public Button buyButton;
        
        public TextMeshProUGUI priceText;
    }
    public enum SkinType
    {
        Character,
        Background
    }

}