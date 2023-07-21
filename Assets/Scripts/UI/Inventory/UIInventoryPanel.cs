using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Inventory
{
    public class UIInventoryPanel : MonoBehaviour
    {
        [SerializeField] private List<Button> _listButton;
        [SerializeField] public List<Items> _items;
        [SerializeField] private Text _description;
        [SerializeField] private Button _currentButton;
        public List<ItemInformation> _itemInfo = new();

        [Serializable]
        public class Items
        {
            public string menuName;
            public List<Button> _listButton;
        }
        public void AddItem(Button button, int dataNumber)
        {
            _items[dataNumber]._listButton.Add(button);
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
            _description.text = _itemInfo[dataNumber].Description;
            _currentButton.Select();
        }
    }
}

