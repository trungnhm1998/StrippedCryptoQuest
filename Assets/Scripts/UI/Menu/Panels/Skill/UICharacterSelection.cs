using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using CryptoQuest.UI.Menu.Character;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UICharacterSelection : MonoBehaviour
    {
        public static UnityAction EnterCharacterSelectionEvent;
        public UnityAction SelectedCharacterEvent;
        public UnityAction<HeroBehaviour, bool> UpdateSkillListEvent;

        [SerializeField] private UICharacterButton _defaultSelection;
        [SerializeField] private UICharacterPartySlot[] _partySlots;

        private IPartyController _party;
        private GameObject _cachedGo;

        private void Awake()
        {
            UICharacterButton.SelectCharacterEvent += InspectSelectedCharacter;
            _party = ServiceProvider.GetService<IPartyController>();
        }

        private void OnDestroy()
        {
            UICharacterButton.SelectCharacterEvent -= InspectSelectedCharacter;
        }

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
            SelectedCharacterEvent?.Invoke();

            // Really bad codes here, might fix later
            if (_cachedGo == selectedCharGo.transform.parent.gameObject) return;
            for (var index = 0; index < _partySlots.Length; index++)
            {
                if (selectedCharGo.transform.parent.gameObject == _partySlots[index].gameObject)
                {
                    UpdateSkillListEvent?.Invoke(_party.Slots[index].HeroBehaviour, true);
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
