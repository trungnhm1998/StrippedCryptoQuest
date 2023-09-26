using System;
using CryptoQuest.UI.Common;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Battle.UI.SelectCommand
{
    public class UISelectCommand : MonoBehaviour
    {
        public event Action AttackCommandPressed;
        public event Action SkillCommandPressed;
        public event Action ItemCommandPressed;
        public event Action GuardCommandPressed;
        public event Action RetreatCommandPressed;
        [SerializeField] private Button _firstButton;
        [SerializeField] private ChildButtonsActivator _childButtonsActivator;
        [SerializeField] private LocalizeStringEvent _characterName;

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

        public void OnAttackPressed() => AttackCommandPressed?.Invoke();

        public void OnSkillPressed() => SkillCommandPressed?.Invoke();

        public void OnItemPressed() => ItemCommandPressed?.Invoke();

        public void OnGuardPressed() => GuardCommandPressed?.Invoke();

        public void OnRetreatPressed() => RetreatCommandPressed?.Invoke();
    }
}