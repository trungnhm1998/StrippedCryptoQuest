using System;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class ConfirmEvolveDialog : MonoBehaviour
    {
        [field: SerializeField] public UIConfirmPanel ConfirmEvolveUI { get; private set; }

        [field: SerializeField] public BlackSmithDialogsPresenter DialogPresenter { get; private set; }

        [SerializeField] private LocalizedString _confirmMessage;

        public event Action Confirmed;
        public event Action Canceling;

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

        private void HandleCancelEvolving() => Canceling?.Invoke();

        private void HandleConfirmEvolving() => Confirmed?.Invoke();

        private void OnDestroy()
        {
            DialogPresenter.ConfirmYesEvent -= HandleConfirmEvolving;
            DialogPresenter.ConfirmNoEvent -= HandleCancelEvolving;
        }
    }
}