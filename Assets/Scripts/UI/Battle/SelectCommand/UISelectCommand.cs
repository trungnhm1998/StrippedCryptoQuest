using CryptoQuest.Battle;
using CryptoQuest.UI.Common;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle.SelectCommand
{
    public class UISelectCommand : MonoBehaviour
    {
        [SerializeField] private BattleStateMachine _stateMachine;
        [SerializeField] private Button _firstButton;
        [SerializeField] private ChildButtonsActivator _childButtonsActivator;
        [SerializeField] private LocalizeStringEvent _characterName;
        [SerializeField] private LocalizedString _testCharacterName;

        private SelectCommandState _selectCommandState;

        private void OnEnable()
        {
            _stateMachine.GoToState(new SelectCommandState(this));
        }

        private void OnDisable() { }

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

        public void OnAttackPressed() { }

        public void OnSkillPressed() { }

        public void OnItemPressed() { }

        public void OnGuardPressed() { }

        public void OnRetreatPressed() { }

        #endregion
    }
}