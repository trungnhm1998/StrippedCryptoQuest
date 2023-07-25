using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Inventory
{
    public class UIInventoryPanel : MonoBehaviour
    {
        [Serializable]
        public class Items
        {
            public List<Button> ListButton;
        }

        [SerializeField] private List<Button> _listButton;
        [SerializeField] private Text _description;
        private Button _currentButton;
        public List<Items> ListItem;
        public List<ItemInformation> ItemInfo = new();

        public void AddItem(Button button, int dataNumber)
        {
            ListItem[dataNumber].ListButton.Add(button);
            _listButton.Add(button);
        }

        public void UpdateButton()
        {
            for (int i = 0; i < _listButton.Count; i++)
            {
                int index = i;
                _listButton[i].onClick.AddListener(() => ButtonClicked(index));
            }
        }

        private void ButtonClicked(int dataNumber)
        {
            _currentButton = _listButton[dataNumber];
            _description.text = ItemInfo[dataNumber].Description;
            _currentButton.Select();
        }
    }
}
