using System;
using CryptoQuest.BlackSmith.Common;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class ConfirmEvolvePresenter : Presenter
    {
        [field: SerializeField] public UIConfirmPanel ConfirmEvolveUI { get; private set; }

        [field: SerializeField] public BlackSmithDialogsPresenter DialogPresenter { get; private set; }

        [SerializeField] private LocalizedString _confirmMessage;

        public event Action OnConfirmEvolving;
        public event Action OnCancelEvolving;

        private void OnEnable()
        {
            ConfirmEvolveUI.gameObject.SetActive(true);
            DialogPresenter.ShowConfirmDialog(_confirmMessage);
            DialogPresenter.ConfirmYesEvent += HandleConfirmEvolving;
            DialogPresenter.ConfirmNoEvent += HandleCancelEvolving;
        }

        private void OnDisable()
        {
            DialogPresenter.HideConfirmDialog();
            ConfirmEvolveUI.gameObject.SetActive(false);
            DialogPresenter.ConfirmYesEvent -= HandleConfirmEvolving;
            DialogPresenter.ConfirmNoEvent -= HandleCancelEvolving;
        }

        private void HandleCancelEvolving()
        {
            OnCancelEvolving?.Invoke();
        }

        private void HandleConfirmEvolving()
        {
            OnConfirmEvolving?.Invoke();
        }

        private void OnDestroy()
        {
            DialogPresenter.ConfirmYesEvent -= HandleConfirmEvolving;
            DialogPresenter.ConfirmNoEvent -= HandleCancelEvolving;
        }
    }
}
