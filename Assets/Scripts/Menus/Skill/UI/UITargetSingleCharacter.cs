using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.UI.Menu.Character;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Skill.UI
{
    public class UITargetSingleCharacter : MonoBehaviour
    {
        public Action<HeroBehaviour> SelectedCharacterEvent;

        [SerializeField] private UICharacterPartySlot[] _partySlots;

        private List<Button> _targetButtons = new();

        private void Init()
        {
            _targetButtons = new();

            foreach (var slot in _partySlots)
            {
                if (!slot.Hero.IsValid()) continue;
                // var selectButton = slot.SelectBorder.GetComponent<Button>();
                // _targetButtons.Add(selectButton);
                // slot.SelectBorder.enabled = true;
                // selectButton.interactable = false;
                // selectButton.onClick.RemoveAllListeners();
                // selectButton.onClick.AddListener(() => SelectCharacter(slot.Hero));
            }
        }

        public void ShowUI()
        {
            Init();
            SetActiveUI(true);
        }

        public void SetActiveUI(bool value)
        {
            foreach (var button in _targetButtons)
            {
                button.interactable = value;
            }

            _targetButtons[0].Select();
        }

        public void SelectCharacter(HeroBehaviour hero)
        {
            SelectedCharacterEvent?.Invoke(hero);
        }
    }
}