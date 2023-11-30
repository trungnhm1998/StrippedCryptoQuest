using System;
using System.Collections.Generic;
using CryptoQuest.Events;
using CryptoQuest.Menus.DimensionalBox.States.EquipmentsTransfer;
using CryptoQuest.Menus.DimensionalBox.States.MagicStoneTransfer;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox.UI.MagicStoneTransfer
{
    public class UIMagicStoneList : MonoBehaviour
    {
        public event Action<UIMagicStone> Transferring;
        public event Action<UIMagicStoneList> Initialized;

        [SerializeField] private bool _wrapAround = true;
        [SerializeField] private ScrollRect _scrollView;
        public ScrollRect ScrollView => _scrollView;
        [SerializeField] private MagicStoneUIPool _pool;

        [Header("Listen to")]
        [SerializeField] private GetMagicStonesEvent _getMagicStonesEvent;

        private List<int> _magicStonesId = new List<int>();
        public List<int> MagicStonesId => _magicStonesId;

        private readonly List<UIMagicStone> _magicStonesToTransfer = new();
        public bool PendingTransfer => _magicStonesToTransfer.Count > 0;
        private TinyMessageSubscriptionToken _confirmTransferEvent;

        private void OnEnable()
        {
            _confirmTransferEvent = ActionDispatcher.Bind<ConfirmTransferMagicStoneAction>(OnTransferEquipment);
            _getMagicStonesEvent.EventRaised += Initialize;

            Initialized?.Invoke(this);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_confirmTransferEvent);
            _getMagicStonesEvent.EventRaised -= Initialize;
        }

        private List<MagicStone> _stones;

        private void Initialize(List<MagicStone> stones)
        {
            _stones = stones;
            ClearOldMagicStones();
            foreach (var equipment in stones)
            {
                var uiMagicStone = _pool.Get();
                uiMagicStone.Pressed += OnTransferring;
                uiMagicStone.transform.SetParent(_scrollView.content);
                uiMagicStone.Initialize(equipment);
            }

            if (stones.Count > 0) Initialized?.Invoke(this);
        }

        public void Transfer(UIMagicStone uiMagicStone)
        {
            if (_stones.Find(item => item.id == uiMagicStone.Id) == null)
            {
                _magicStonesToTransfer.Add(uiMagicStone);
                uiMagicStone.EnablePendingTag(true);
            }

            uiMagicStone.Pressed += OnTransferring;
            uiMagicStone.transform.SetParent(_scrollView.content);
            uiMagicStone.transform.SetAsLastSibling();
            CurrentSelectedIndex = _scrollView.content.transform.childCount - 1;
        }

        private void ClearOldMagicStones()
        {
            CurrentSelectedIndex = 0;
            _magicStonesToTransfer.Clear();
            var oldEquipments = _scrollView.content.GetComponentsInChildren<UIMagicStone>();
            foreach (var equipment in oldEquipments)
            {
                equipment.Pressed -= OnTransferring;
                _pool.Release(equipment);
            }
        }

        public void OnTransferring(UIMagicStone ui)
        {
            if (_magicStonesToTransfer.Remove(ui)) ui.EnablePendingTag(false);
            CurrentSelectedIndex -= 1;
            ui.Pressed -= OnTransferring;
            Transferring?.Invoke(ui);
        }

        public bool Focus(bool resetToTop = false)
        {
            if (_scrollView.content.transform.childCount == 0) return false;
            CurrentSelectedIndex = resetToTop ? 0 : CurrentSelectedIndex;
            SelectChild(CurrentSelectedIndex);
            return true;
        }

        private int _currentSelectedIndex;

        public int CurrentSelectedIndex
        {
            get => _currentSelectedIndex;
            set
            {
                _currentSelectedIndex = value;
                var childCount = _scrollView.content.transform.childCount;
                if (childCount == 0)
                {
                    _currentSelectedIndex = 0;
                    return;
                }

                if (_currentSelectedIndex >= 0 &&
                    _currentSelectedIndex < childCount) return;
                _currentSelectedIndex = _wrapAround
                    ? (_currentSelectedIndex + childCount) % childCount
                    : Mathf.Clamp(_currentSelectedIndex, 0, childCount - 1);
            }
        }

        public void Navigate(float dirY)
        {
            if (dirY == 0) return;
            CurrentSelectedIndex += (int)dirY;
            SelectChild(CurrentSelectedIndex);
        }

        private void SelectChild(int selectedIndex)
        {
            var childToSelect = _scrollView.content.transform.GetChild(selectedIndex);
            if (childToSelect == null) childToSelect = _scrollView.content.transform.GetChild(0);
            EventSystem.current.SetSelectedGameObject(childToSelect.gameObject);
        }

        private void OnTransferEquipment(ConfirmTransferMagicStoneAction confirmTransferAction)
        {
            if (_magicStonesToTransfer.Count == 0) return;
            foreach (var stone in _magicStonesToTransfer) _magicStonesId.Add(stone.Id);
        }

        public void Reset() => Initialize(_stones);
    }
}