using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Menu;
using CryptoQuest.System;
using CryptoQuest.UI.Menu.Character;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIItemCharacterSelection : MonoBehaviour
    {
        public event Action<int> Clicked;
        [SerializeField] private ServiceProvider _serviceProvider;
        [SerializeField] private UICharacterPartySlot[] _partySlots;
        [SerializeField] private List<MultiInputButton> _characterButtons;

        private IParty _party;

        private void Awake()
        {
            _party = _serviceProvider.PartyController.Party;
            _serviceProvider.PartyProvided += InitParty;
        }

        private void OnDestroy()
        {
            _serviceProvider.PartyProvided -= InitParty;
        }

        private void OnEnable() 
        {
            LoadPartyMembers();
        }

        private void LoadPartyMembers()
        {
            for (var index = 0; index < _party.Members.Length; index++)
            {
                var member = _party.Members[index];
                var slot = _partySlots[index];
                slot.Active(member.IsValid());
                if (!member.IsValid()) continue;
                slot.Init(member);
            }
        }

        public void Init()
        {
            EnableAllButtons();
            _characterButtons[0].Select();
        }

        public void DeInit()
        {
            DisableAllButtons();
        }

        /// <summary>
        /// Add buttons to unity event system
        /// </summary>
        private void EnableAllButtons()
        {
            foreach (var button in _characterButtons)
            {
                button.enabled = true;
            }
        }

        /// <summary>
        /// Remove buttons from unity event system
        /// </summary>
        private void DisableAllButtons()
        {
            foreach (var button in _characterButtons)
            {
                button.enabled = false;
            }
        }

        public void OnClicked(int index)
        {
            Clicked?.Invoke(index);
        }

        private void InitParty(IPartyController partyController)
        {
            _party = _serviceProvider.PartyController.Party;
            LoadPartyMembers();
        }
    }
}