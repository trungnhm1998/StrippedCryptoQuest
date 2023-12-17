using System;
using System.Linq;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class UIAttachList : MonoBehaviour
    {
        public event UnityAction SelectStoneToAttachEvent;

        [SerializeField] private InputMediatorSO _input;
        [SerializeField] private UIAttachSlot[] _attachSlots; // Max slots = 7

        private int _currentIndex = 0;
        private int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                int count = _slots;
                _currentIndex = (value + count) % count;
            }
        }

        private int _slots;

        public void Init(int slots)
        {
            _slots = slots;
            for (var i = 0; i < slots; i++)
            {
                _attachSlots[i].gameObject.SetActive(true);
                _attachSlots[i].Pressed += OnSelectStoneToAttach;
            }
        }

        private void OnSelectStoneToAttach() => SelectStoneToAttachEvent?.Invoke();

        private void Navigate(Vector2 dir)
        {
            switch (dir.y)
            {
                case > 0:
                    GoUp();
                    break;
                case < 0:
                    GoDown();
                    break;
            }
        }

        private void GoUp()
        {
            CurrentIndex--;
            SelectCurrentSlot();
        }

        private void GoDown()
        {
            CurrentIndex++;
            SelectCurrentSlot();
        }

        private void SelectCurrentSlot()
        {
            var slotButton = _attachSlots[CurrentIndex].GetComponent<Button>();
            slotButton.Select();
        }

        private void ResetUI()
        {
            _input.MenuNavigateEvent -= Navigate;
            foreach (var slot in _attachSlots)
            {
                if (slot == null) return;
                slot.gameObject.SetActive(false);
                slot.Pressed -= OnSelectStoneToAttach;
            }
        }

        public void EnterSlotSelection()
        {
            _input.MenuNavigateEvent += Navigate;
            _attachSlots[CurrentIndex].UnCache();
            SelectCurrentSlot();
            SetActiveAllSlotButtons(true);
        }

        public void ExitSlotSelection()
        {
            _input.MenuNavigateEvent -= Navigate;
            _attachSlots[CurrentIndex].Cache();
            SetActiveAllSlotButtons(false);
        }

        private void SetActiveAllSlotButtons(bool isActive)
        {
            var slotButtons = gameObject.GetComponentsInChildren<Button>();
            foreach (var button in slotButtons)
                button.enabled = isActive;
        }

        private void OnDisable() => ResetUI();
        private void OnDestroy() => ResetUI();
    }
}