using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Quest;
using CryptoQuest.Input;
using CryptoQuest.UI.Dialogs.BattleDialog;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs.LevelStatusDialog
{
    public class UILevelStatusDialog : ModalWindow<UILevelStatusDialog>
    {
        [Header("Child Components")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private Button _defaultSelectButton;
        [SerializeField] private LevelUpCharName _levelStatusTextPrefab;
        [SerializeField] private Transform _levelStatusesContainerTransform;
        [SerializeField] private LocalizedString _noLevelUpTargetString;

        public UnityAction CloseButtonPressed;
        private LevelStatusDialogData _levelStatusDialogData;

        protected override void OnBeforeShow()
        {
            DisplayListLeveledUpCharacter();
        }

        protected override void CheckIgnorableForClose() { }

        private void Awake()
        {
            StartCoroutine(CoSelectDefaultButton());
        }

        private IEnumerator CoSelectDefaultButton()
        {
            yield return null;
            _defaultSelectButton.Select();
        }

        public void OnCloseButtonPressed()
        {
            CloseButtonPressed.Invoke();
            Close();
        }

        public UILevelStatusDialog SetDialog(LevelStatusDialogData levelStatusDialogData)
        {
            _levelStatusDialogData = levelStatusDialogData;
            return this;
        }

        private void DisplayListLeveledUpCharacter()
        {
            foreach (var target in _levelStatusDialogData.TargetTextList)
            {
                SetNameForLeveledUpCharacter(target);
            }
        }

        private void SetNameForLeveledUpCharacter(LocalizedString name)
        {
            var component = Instantiate<LevelUpCharName>(_levelStatusTextPrefab, _levelStatusesContainerTransform);
            component.SetName(name);
        }
    }

}
