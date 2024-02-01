using System.Collections;
using CryptoQuest.Input;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class UIAttachList : MonoBehaviour
    {
        public event UnityAction<IMagicStone> AttachSlotSelectedEvent;

        [SerializeField] private InputMediatorSO _input;
        [SerializeField] private MagicStoneInventory _inventory;
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
                _attachSlots[i].Pressed += OnAttachSlotSelected;
            }
        }

        private void OnAttachSlotSelected()
        {
            var singleStoneUi = _attachSlots[CurrentIndex].SingleStoneUI;
            var currentAttachedStone = singleStoneUi.Data;
            if (!singleStoneUi.gameObject.activeSelf) currentAttachedStone = null;
            AttachSlotSelectedEvent?.Invoke(currentAttachedStone);
        }

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
                slot.gameObject.SetActive(false);
                slot.Pressed -= OnAttachSlotSelected;
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

        public void RenderCurrentAttachedStones(IEquipment equipment)
        {
            DetachAllAttachSlots();

            var attachedStoneIDs = equipment.Data.AttachStones;
            for (var i = 0; i < attachedStoneIDs.Count; i++)
            {
                foreach (var magicStone in _inventory.MagicStones)
                {
                    if (attachedStoneIDs[i] != magicStone.ID) continue;
                    _attachSlots[i].Attach(magicStone);
                }
            }
        }

        public void DetachCurrent()
        {
            _attachSlots[CurrentIndex].Detach();
        }

        private void DetachAllAttachSlots()
        {
            foreach (var slot in _attachSlots)
            {
                slot.Detach();
            }
        }

        private void SetActiveAllSlotButtons(bool isActive)
        {
            var slotButtons = gameObject.GetComponentsInChildren<Button>();
            foreach (var button in slotButtons)
                button.enabled = isActive;
        }

        public void AttachStoneToCurrentSlot(IMagicStone stoneData) => _attachSlots[CurrentIndex].Attach(stoneData);
        private void OnDisable() => ResetUI();
        private void OnDestroy() => ResetUI();
    }
}