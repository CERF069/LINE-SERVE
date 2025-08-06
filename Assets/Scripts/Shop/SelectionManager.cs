using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class SelectionManager : MonoBehaviour
    {
        private const string SelectedCharacterKey = "SelectedCharacter";
        private const string SelectedBackgroundKey = "SelectedBackground";

        private string _selectedCharacterId;
        private string _selectedBackgroundId;

        [Header("UI")]
        [SerializeField] private List<Button> characterButtons;
        [SerializeField] private List<Button> backgroundButtons;
        [SerializeField] private List<string> characterIds;
        [SerializeField] private List<string> backgroundIds;

        
        [SerializeField] private SpriteRenderer characterRenderer;
        [SerializeField] private SpriteRenderer backgroundRenderer;
        [SerializeField] private ShopManager shopManager;
        
        
        private void Start()
        {
            EnsureDefaultItemsBought(); // Добавлено

            SelectCharacter(PlayerPrefs.GetString(SelectedCharacterKey, "Human_Def"));
            SelectBackground(PlayerPrefs.GetString(SelectedBackgroundKey, "Background_Def"));

            UpdateManager();
        }

        private void EnsureDefaultItemsBought()
        {
            if (PlayerPrefs.GetInt("ShopItem_Human_Def", 0) == 0)
            {
                PlayerPrefs.SetInt("ShopItem_Human_Def", 1);
            }

            if (PlayerPrefs.GetInt("ShopItem_Background_Def", 0) == 0)
            {
                PlayerPrefs.SetInt("ShopItem_Background_Def", 1);
            }

            PlayerPrefs.Save();
        }


        public void UpdateManager()
        {
            _selectedCharacterId = PlayerPrefs.GetString(SelectedCharacterKey, "Human_Def");
            _selectedBackgroundId = PlayerPrefs.GetString(SelectedBackgroundKey, "Background_Def");

            for (int i = 0; i < characterButtons.Count; i++)
            {
                int index = i;
                string id = characterIds[index];

                characterButtons[index].onClick.AddListener(() => TrySelectCharacter(id));
            }

            for (int i = 0; i < backgroundButtons.Count; i++)
            {
                int index = i;
                string id = backgroundIds[index];

                backgroundButtons[index].onClick.AddListener(() => TrySelectBackground(id));
            }

            UpdateCharacterButtonStates();
            UpdateBackgroundButtonStates();
        }

        private void TrySelectCharacter(string id)
        {
            if (!IsItemBought(id)) return;

            SelectCharacter(id);
            UpdateCharacterButtonStates();
        }

        private void TrySelectBackground(string id)
        {
            if (!IsItemBought(id)) return;

            SelectBackground(id);
            UpdateBackgroundButtonStates();
        }
        private bool IsItemBought(string itemId)
        {
            return PlayerPrefs.GetInt($"ShopItem_{itemId}", 0) == 1;
        }

        public void SelectCharacter(string id)
        {
            _selectedCharacterId = id;
            PlayerPrefs.SetString(SelectedCharacterKey, id);
            PlayerPrefs.Save();
            Debug.Log($"Character selected: {id}");

            var sprite = shopManager.GetSpriteById(id);
            if (sprite != null && characterRenderer != null)
                characterRenderer.sprite = sprite;
        }

        public void SelectBackground(string id)
        {
            _selectedBackgroundId = id;
            PlayerPrefs.SetString(SelectedBackgroundKey, id);
            PlayerPrefs.Save();
            Debug.Log($"Background selected: {id}");

            var sprite = shopManager.GetSpriteById(id);
            if (sprite != null && backgroundRenderer != null)
                backgroundRenderer.sprite = sprite;
        }


        private void UpdateCharacterButtonStates()
        {
            for (int i = 0; i < characterButtons.Count; i++)
            {
                string id = characterIds[i];
                var button = characterButtons[i];
                bool isSelected = id == _selectedCharacterId;
                bool isBought = IsItemBought(id);

                button.interactable = isBought && !isSelected;

                if (!isBought)
                {
                    button.image.color = Color.black; // не куплен — чёрный
                }
                else if (isSelected)
                {
                    button.image.color = new Color(0.7f, 1f, 0.7f); // выбран — светло-зеленый
                }
                else
                {
                    button.image.color = Color.white; // куплен, но не выбран — белый
                }
            }
        }

        private void UpdateBackgroundButtonStates()
        {
            for (int i = 0; i < backgroundButtons.Count; i++)
            {
                string id = backgroundIds[i];
                var button = backgroundButtons[i];
                bool isSelected = id == _selectedBackgroundId;
                bool isBought = IsItemBought(id);

                button.interactable = isBought && !isSelected;

                if (!isBought)
                {
                    button.image.color = Color.black; // не куплен — чёрный
                }
                else if (isSelected)
                {
                    button.image.color = new Color(0.7f, 1f, 0.7f); // выбран — светло-зеленый
                }
                else
                {
                    button.image.color = Color.white; // куплен, но не выбран — белый
                }
            }
        }

    }
}
