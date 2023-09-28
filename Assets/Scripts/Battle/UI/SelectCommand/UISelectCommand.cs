using CryptoQuest.UI.Common;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

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
        [SerializeField] private Button _firstButton;
        [SerializeField] private ChildButtonsActivator _childButtonsActivator;
        [SerializeField] private LocalizeStringEvent _characterName;
        private ISelectCommandCallback _commandCallback;

        public void SetCharacterName(LocalizedString characterName)
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
        
        public void RegisterCallback(ISelectCommandCallback commandCallback) => _commandCallback = commandCallback;

        public void OnAttackPressed() => _commandCallback?.OnAttackPressed();

        public void OnSkillPressed() => _commandCallback?.OnSkillPressed();

        public void OnItemPressed() => _commandCallback?.OnItemPressed();

        public void OnGuardPressed() => _commandCallback?.OnGuardPressed();

        public void OnRetreatPressed() => _commandCallback?.OnRetreatPressed();
    }
}