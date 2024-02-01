using System.Collections;
using CryptoQuest.Gameplay.Quest;
using CryptoQuest.Input;
using CryptoQuest.Quest.Controllers;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
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

        private LevelStatusDialogData _levelStatusDialogData;

        protected override void OnBeforeShow()
        {
            DisplayListLeveledUpCharacter();
        }

        protected override void CheckIgnorableForClose() { }

        private void Awake()
        {
            _inputMediator.DisableAllInput();
            StartCoroutine(CoSelectDefaultButton());
        }

        private IEnumerator CoSelectDefaultButton()
        {
            yield return null;
            _defaultSelectButton.Select();
        }

        public void OnCloseButtonPressed()
        {
            Close();
            _inputMediator.EnableMapGameplayInput();
            ActionDispatcher.Dispatch(new ResumeCutsceneAction());
        }

        public override UILevelStatusDialog Close()
        {
            if (this == null) return this;
            Visible = false;
            Destroy(gameObject);
            return this;
        }

        public UILevelStatusDialog SetDialog(LevelStatusDialogData levelStatusDialogData)
        {
            _levelStatusDialogData = levelStatusDialogData;
            return this;
        }

        private void DisplayListLeveledUpCharacter()
        {
            CleanUpTargets();
            foreach (var target in _levelStatusDialogData.TargetTextList)
            {
                SetNameForLeveledUpCharacter(target);
            }
            _levelStatusDialogData.ClearListTarget();
        }

        private void SetNameForLeveledUpCharacter(LocalizedString name)
        {
            var component = Instantiate<LevelUpCharName>(_levelStatusTextPrefab, _levelStatusesContainerTransform);
            component.SetName(name);
        }

        private void CleanUpTargets()
        {
            foreach (Transform child in _levelStatusesContainerTransform) Destroy(child.gameObject);
        }
    }

}
