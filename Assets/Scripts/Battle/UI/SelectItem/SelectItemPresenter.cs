using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.Helper;
using CryptoQuest.Input;
using CryptoQuest.System;
using CryptoQuest.UI.Common;
using DG.Tweening;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Battle.UI.SelectItem
{
    public class SelectItemPresenter : MonoBehaviour
    {
        public delegate void ItemTargetTypeDelegate(UIItem item);

        public ItemTargetTypeDelegate SelectSingleEnemyCallback { get; set; }
        private void OnSingleEnemy() => SelectSingleEnemyCallback?.Invoke(_lastSelectedItem);

        public ItemTargetTypeDelegate SelectSingleHeroCallback { get; set; }
        private void OnSingleHero() => SelectSingleHeroCallback?.Invoke(_lastSelectedItem);

        public ItemTargetTypeDelegate SelectAllEnemyCallback { get; set; }
        private void OnTargetAllEnemy() => SelectAllEnemyCallback?.Invoke(_lastSelectedItem);
        
        public ItemTargetTypeDelegate SelectAllHeroCallback { get; set; }
        private void OnTargetAllHero() => SelectAllHeroCallback?.Invoke(_lastSelectedItem);


        [SerializeField] private AutoScroll _autoScroll;
        [SerializeField] private BattleInputSO _input;
        [SerializeField] private ScrollRect _itemScroll;
        [SerializeField] private UIItem _itemPrefab;

        [Header("State event context")]
        [SerializeField] private VoidEventChannelSO _singleHeroChannel;
        [SerializeField] private VoidEventChannelSO _singleEnemyChannel;
        [SerializeField] private VoidEventChannelSO _allEnemyChannel;
        [SerializeField] private VoidEventChannelSO _allHeroChannel;

        private UIItem _lastSelectedItem;
        private UIItem _firstItem;
        public UIItem LastSelectedItem => _lastSelectedItem;
        
        private readonly List<UIItem> _items = new List<UIItem>();

        private void OnDisable()
        {
            UnregisterEvents();
        }

        public void Show()
        {
            InitItemButtons();
            DOVirtual.DelayedCall(0.1f, SelectFirstOrLastSelectedSkill);
            SetActiveScroll(true);
            RegisterEvents();
        }

        public void SetActiveScroll(bool value)
        {
            _itemScroll.gameObject.SetActive(value);
        }

        private void SelectFirstOrLastSelectedSkill()
        {
            if (_lastSelectedItem != null)
            {
                EventSystem.current.SetSelectedGameObject(_lastSelectedItem.gameObject);
                _lastSelectedItem = null;
                return;
            }
            EventSystem.current.SetSelectedGameObject(_firstItem.gameObject);
        }

        private void RegisterEvents()
        {
            _input.NavigateEvent += UpdateAutoScroll;
            _singleHeroChannel.EventRaised += OnSingleHero;
            _singleEnemyChannel.EventRaised += OnSingleEnemy;
            _allEnemyChannel.EventRaised += OnTargetAllEnemy;
            _allHeroChannel.EventRaised += OnTargetAllHero;
        }

        private void UnregisterEvents()
        {
            _input.NavigateEvent -= UpdateAutoScroll;
            _singleHeroChannel.EventRaised -= OnSingleHero;
            _singleEnemyChannel.EventRaised -= OnSingleEnemy;
            _allEnemyChannel.EventRaised -= OnTargetAllEnemy;
            _allHeroChannel.EventRaised -= OnTargetAllHero;
        }

        private void InitItemButtons()
        {
            if (_items.Count > 0) return;
            DestroyAllItems();
            var inventory = ServiceProvider.GetService<IInventoryController>().Inventory;
            foreach (var item in inventory.GetItemsInBattle())
            {
                var itemUI = Instantiate<UIItem>(_itemPrefab, _itemScroll.content);
                itemUI.Selected += SelectingTarget;
                _items.Add(itemUI);
                itemUI.Init(item);
                if (_firstItem == null) _firstItem = itemUI;
            }
        }

        public void Hide()
        {
            SetActiveScroll(false);
            UnregisterEvents();
        }

        private void SelectingTarget(UIItem itemUI)
        {
            _lastSelectedItem = itemUI;
            itemUI.Item.Consuming();
        }
        
        private void DestroyAllItems()
        {
            foreach (var item in _items)
            {
                item.Selected -= SelectingTarget;
                Destroy(item.gameObject);
            }

            _items.Clear();
        }

        private void UpdateAutoScroll(Vector2 direction)
        {
            _autoScroll.Scroll();
        }
    }
}