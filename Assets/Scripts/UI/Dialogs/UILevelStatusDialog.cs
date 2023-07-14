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

namespace CryptoQuest.UI.Dialogs
{
    public class UILevelStatusDialog : ModalWindow<UILevelStatusDialog>
    {
        [Header("Child Components")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private Button _defaultSelectButton;
        [SerializeField] private Test _levelStatusTextPrefab;
        [SerializeField] private Transform _levelStatusesContainerTransform;
        [SerializeField] private LocalizedString _noLevelUpTargetString;

        public UnityAction CloseButtonPressed;
        private LevelStatusDialogData _levelStatusDialogData;

        protected override void OnBeforeShow()
        {
            DisplayItemsReward();
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

        private void DisplayItemsReward()
        {
            // if (_levelStatusDialogData.TargetTextList.Count <= 0)
            // {
            //     SetNameOfLevelUpCharacterLabel(_noLevelUpTargetString);
            //     return;
            // }

            foreach (var target in _levelStatusDialogData.TargetTextList)
            {
                SetNameOfLevelUpCharacterLabel(target);
            }
        }

        private LocalizeStringEvent GetNameOfLevelUpCharacterLabel()
        {
            var textGO = Instantiate(_levelStatusTextPrefab, _levelStatusesContainerTransform);

            var levelUpTargetLabel = textGO.GetComponent<LocalizeStringEvent>();

            return levelUpTargetLabel;
        }

        private void SetNameOfLevelUpCharacterLabel(LocalizedString name)
        {
            // var levelUpTargetLabel = 
            //     GetNameOfLevelUpCharacterLabel();

            // levelUpTargetLabel.StringReference = levelUpTargetString;

            var component = Instantiate<Test>(_levelStatusTextPrefab, _levelStatusesContainerTransform);
            component.SetName(name);


        }
    }

}
