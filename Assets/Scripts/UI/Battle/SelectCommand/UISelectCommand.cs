using System;
using CryptoQuest.UI.Battle.CommandsMenu;
using CryptoQuest.UI.Battle.StateMachine;
using FSM;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle.SelectCommand
{
    public class UISelectCommand : MonoBehaviour, IBattleMenu
    {
        public static Action<LocalizedString> RequestSetCharacterName;
        public static event Action SelectedAttack;
        public static event Action SelectedSkill;
        public static event Action SelectedItem;
        public static event Action SelectedGuard;
        public static event Action SelectedRetreat;
        
        public static readonly string SelectCommandState = "SelectCommand";
        public string StateName => SelectCommandState;

        [SerializeField] private Button _firstButton;
        [SerializeField] private ChildButtonsActivator _childButtonsActivator;
        [SerializeField] private LocalizeStringEvent _characterName;
        [SerializeField] private LocalizedString _testCharacterName;

        private SelectCommandState _selectCommandState;

        public StateBase<string> CreateState(BattleMenuStateMachine machine)
        {
            _selectCommandState = new SelectCommandState(machine, this);
            return _selectCommandState;
        }

        private void OnEnable()
        {
            RequestSetCharacterName += SetCharacterName;
            RequestSetCharacterName?.Invoke(_testCharacterName);
        }

        private void OnDisable()
        {
            RequestSetCharacterName -= SetCharacterName;
        }

        private void SetCharacterName(LocalizedString characterName)
        {
            _characterName.StringReference = characterName;
        }

        public void SelectFirstButton()
        {
            _firstButton.Select();
        }

        public void SetActiveCommandsMenu(bool isActive)
        {
            _childButtonsActivator.SetActiveButtons(isActive);
        }

        #region Method setup in UI

        public void OnAttackPressed()
        {
            SelectedAttack?.Invoke();
        }

        public void OnSkillPressed()
        {
            SelectedSkill?.Invoke();
            _selectCommandState.ChangeToSkillState();
        }

        public void OnItemPressed()
        {
            SelectedItem?.Invoke();
            _selectCommandState.ChangeToItemState();
        }

        public void OnGuardPressed()
        {
            SelectedGuard?.Invoke();
        }

        public void OnRetreatPressed()
        {
            SelectedRetreat?.Invoke();
        }

        #endregion
    }
}