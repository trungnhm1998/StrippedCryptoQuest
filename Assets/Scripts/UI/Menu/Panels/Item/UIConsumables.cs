using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using PolyAndCode.UI;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIConsumables : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        [SerializeField] private RecyclableScrollRect _recyclableScrollRect;
        [SerializeField] private GameObject _content;
        [field: SerializeField] public UsableTypeSO Type { get; private set; }
        private IConsumablesProvider _consumablesProvider;

        private void Awake()
        {
            _consumablesProvider = GetComponent<IConsumablesProvider>(); // TODO: Find a better way to inject
            _uiConsumables = new UIConsumableItem[GetItemCount()];
            _recyclableScrollRect.Initialize(this);
        }

        #region PLUGINS

        public int GetItemCount()
        {
            return _consumablesProvider.Items.Count;
        }

        private UIConsumableItem[] _uiConsumables;

        public void SetCell(ICell cell, int index)
        {
            try
            {
                var item = cell as UIConsumableItem;
                item.Init(_consumablesProvider.Items[index]);
                _uiConsumables[index] = item;
                if (_showing && index == 0)
                {
                    item.Inspect();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to set cell at index {index} with error: {e.Message}");
                throw;
            }
        }

        #endregion

        public void Hide()
        {
            _content.SetActive(false);
        }

        private bool _showing;
        public void Show()
        {
            _content.SetActive(true);
            _showing = true;
        }
    }
}