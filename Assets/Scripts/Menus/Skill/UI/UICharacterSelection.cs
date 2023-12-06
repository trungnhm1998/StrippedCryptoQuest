using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using CryptoQuest.UI.Menu.Character;
using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.Menus.Skill.UI
{
    public class UICharacterSelection : MonoBehaviour
    {
        [SerializeField] private UISkillCharacterPartySlot[] _partySlots;

        private IPartyController _party;

        private void Awake()
        {
            _party = ServiceProvider.GetService<IPartyController>();
        }

        private void OnDestroy() { }

        private void OnEnable()
        {
            LoadPartyMembers();
        }

        private void LoadPartyMembers()
        {
            for (var index = 0; index < _party.Slots.Length; index++)
            {
                var member = _party.Slots[index];
                var ui = _partySlots[index];

                ui.gameObject.SetActive(member.IsValid());
                if (!member.IsValid()) continue;
                ui.Init(member.HeroBehaviour, index);

                if (index == 0) ui.Select();
            }
        }
    }
}