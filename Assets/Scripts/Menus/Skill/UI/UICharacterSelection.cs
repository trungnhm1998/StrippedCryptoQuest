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
        private GameObject _cachedGo;

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
            }

            _cachedGo = _partySlots[0].gameObject; // Bad code
        }

        private void InspectSelectedCharacter(GameObject selectedCharGo)
        {
            // Really bad codes here, might fix later
            if (_cachedGo == selectedCharGo.transform.parent.gameObject) return;
            for (var index = 0; index < _partySlots.Length; index++)
            {
                if (selectedCharGo.transform.parent.gameObject == _partySlots[index].gameObject)
                {
                    _cachedGo = _partySlots[index].gameObject;
                    return;
                }
            }
        }
    }
}