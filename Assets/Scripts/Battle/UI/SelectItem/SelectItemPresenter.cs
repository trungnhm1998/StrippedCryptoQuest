using System;
using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.Helper;
using CryptoQuest.Item;
using CryptoQuest.Item.Consumable;
using CryptoQuest.System;
using IndiGames.Core.Common;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.UI.SelectItem
{
    public class SelectItemPresenter : MonoBehaviour
    {
        public event Action<ConsumableInfo> SelectedItemEvent;
        public delegate void ItemTargetTypeDelegate(ConsumableInfo item);

        public ItemTargetTypeDelegate SelectSingleEnemyCallback { get; set; }
        private void OnSingleEnemy() => SelectSingleEnemyCallback?.Invoke(_selectedItem);

        public ItemTargetTypeDelegate SelectSingleHeroCallback { get; set; }
        private void OnSingleHero() => SelectSingleHeroCallback?.Invoke(_selectedItem);

        public ItemTargetTypeDelegate SelectAllEnemyCallback { get; set; }
        private void OnTargetAllEnemy() => SelectAllEnemyCallback?.Invoke(_selectedItem);
        
        public ItemTargetTypeDelegate SelectAllHeroCallback { get; set; }
        private void OnTargetAllHero() => SelectAllHeroCallback?.Invoke(_selectedItem);

        [SerializeField] private UICommandDetailPanel _itemListUI;

        [Header("State event context")]
        [SerializeField] private VoidEventChannelSO _singleHeroChannel;
        [SerializeField] private VoidEventChannelSO _singleEnemyChannel;
        [SerializeField] private VoidEventChannelSO _allEnemyChannel;
        [SerializeField] private VoidEventChannelSO _allHeroChannel;

        private ConsumableInfo _selectedItem;
        public ConsumableInfo SelectedItem => _selectedItem;

        private void OnEnable()
        {
            SetInteractive(false);
        }

        public void Show()
        {
            SetInteractive(true);
            ShowItemListUI();
            SetActiveScroll(true);
            RegisterEvents();
        }

        public void SetInteractive(bool value)
        {
            _itemListUI.Interactable = value;
        }

        public void SetActiveScroll(bool value)
        {
            _itemListUI.SetActiveContent(value);
        }

        private void ShowItemListUI()
        {
            var inventory = ServiceProvider.GetService<IInventoryController>().Inventory;
            var model = new CommandDetailModel();

            foreach (var item in inventory.GetItemsInBattle())
            {
                var itemButtonInfo = new ItemButtonInfo(item, ConfirmSelectItem);
                model.AddInfo(itemButtonInfo);
            }

            _itemListUI.ShowCommandDetail(model);
        }

        private void ConfirmSelectItem(ConsumableInfo item)
        {
            _selectedItem = item;
            SelectedItemEvent?.Invoke(item);
            item.Consuming();
        }

        public void Hide()
        {
            SetInteractive(false);
            SetActiveScroll(false);
            UnregisterEvents();
        }
        
        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            _singleHeroChannel.EventRaised += OnSingleHero;
            _singleEnemyChannel.EventRaised += OnSingleEnemy;
            _allEnemyChannel.EventRaised += OnTargetAllEnemy;
            _allHeroChannel.EventRaised += OnTargetAllHero;
        }

        private void UnregisterEvents()
        {
            _singleHeroChannel.EventRaised -= OnSingleHero;
            _singleEnemyChannel.EventRaised -= OnSingleEnemy;
            _allEnemyChannel.EventRaised -= OnTargetAllEnemy;
            _allHeroChannel.EventRaised -= OnTargetAllHero;
        }
    }

    [Serializable]
    public class ItemButtonInfo : ButtonInfoBase
    {
        public ConsumableInfo Item { get; private set; }
        private Action<ConsumableInfo> _itemCallback;

        public ItemButtonInfo(ConsumableInfo item, Action<ConsumableInfo> itemCallback)
            : base("", $"x{item.Quantity.ToString()}")
        {
            Item = item;
            LocalizedLabel = item.DisplayName;
            _itemCallback = itemCallback;
        }

        public override void OnHandleClick()
        {
            _itemCallback?.Invoke(Item);
        }

        public override void Accept(IButtonUI ui)
        {
            ui.Visit(this);
        }
    }
}