using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UICharacterSelection : MonoBehaviour
    {
        [SerializeField] private ServiceProvider _serviceProvider;
        [SerializeField] private UICharacterButton _defaultSelection;
        [SerializeField] private UISkillPartySlot[] _partySlots;

        private IParty _party;

        private void Awake()
        {
            _party = _serviceProvider.PartyController.Party;
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
            _defaultSelection.Select();
        }

        public void DeInit()
        {
        }
    }
}
