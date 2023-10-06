using CryptoQuest.UI.Common;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace CryptoQuest.Battle.UI.SelectCommand
{
    public interface ISelectCommandCallback
    {
        public void OnAttackPressed();
        public void OnSkillPressed();
        public void OnItemPressed();
        public void OnGuardPressed();
        public void OnRetreatPressed();
    }

    public class UISelectCommand : MonoBehaviour
    {
        [SerializeField] private ChildButtonsActivator _childButtonsActivator;
        [SerializeField] private VerticalButtonSelector _buttonSelector;
        [SerializeField] private LocalizeStringEvent _characterName;
        private ISelectCommandCallback _commandCallback;

        public void SetCharacterName(LocalizedString characterName)
        {
            _characterName.StringReference = characterName;
        }

        public void SetActiveCommandsMenu(bool isActive)
        {
            _buttonSelector.Interactable = isActive;
            _childButtonsActivator.SetActiveButtons(isActive);
        }
        
        public void RegisterCallback(ISelectCommandCallback commandCallback) => _commandCallback = commandCallback;

        public void OnAttackPressed() => _commandCallback?.OnAttackPressed();

        public void OnSkillPressed() => _commandCallback?.OnSkillPressed();

        public void OnItemPressed() => _commandCallback?.OnItemPressed();

        public void OnGuardPressed() => _commandCallback?.OnGuardPressed();

        public void OnRetreatPressed() => _commandCallback?.OnRetreatPressed();

        public void SelectFirstButton()
        {
            _buttonSelector.SelectFirstButton();
        }
    }
}