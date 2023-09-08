using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UICharacterSelection : MonoBehaviour
    {
        public static UnityAction EnterCharacterSelectionEvent;
        public UnityAction SelectedCharacterEvent;
        public UnityAction<CharacterSpec, bool> UpdateSkillListEvent;

        [SerializeField] private ServiceProvider _serviceProvider;
        [SerializeField] private UICharacterButton _defaultSelection;
        [SerializeField] private UISkillPartySlot[] _partySlots;

        private IParty _party;
        private GameObject _cachedGo;

        private void Awake()
        {
            UICharacterButton.SelectCharacterEvent += InspectSelectedCharacter;

            _party = _serviceProvider.PartyController.Party;
            LoadPartyMembers();
        }

        private void OnDestroy()
        {
            UICharacterButton.SelectCharacterEvent -= InspectSelectedCharacter;
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

            _cachedGo = _partySlots[0].transform.GetChild(0).gameObject; // Bad code
        }

        private void InspectSelectedCharacter(GameObject selectedCharGo)
        {
            SelectedCharacterEvent?.Invoke();

            // Really bad codes here, might fix later
            if (_cachedGo == selectedCharGo.transform.parent.gameObject) return;
            for (var index = 0; index < _partySlots.Length; index++)
            {
                
                if (selectedCharGo.transform.parent.gameObject == _partySlots[index].gameObject)
                {
                    UpdateSkillListEvent?.Invoke(_party.Members[index], true);
                    _cachedGo = _partySlots[index].gameObject;
                    return;
                }
            }
        }

        #region CharacterSelectionState Setup
        public void Init()
        {
            _defaultSelection.Select();
            EnterCharacterSelectionEvent?.Invoke();
        }
        #endregion
    }
}
