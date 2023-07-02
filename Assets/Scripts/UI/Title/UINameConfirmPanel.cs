using System.Collections;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.UI.Title
{
    public class UINameConfirmPanel : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private Button _yesButton;

        public UnityAction YesButtonPressed;
        public UnityAction NoButtonPressed;

        private void OnEnable()
        {
            StartCoroutine(CoSelectYesButton());
            _inputMediator.CancelEvent += OnNoButtonPressed;
        }

        private IEnumerator CoSelectYesButton()
        {
            yield return new WaitForSeconds(.03f);
            _yesButton.Select();
        }

        private void OnDisable()
        {
            _inputMediator.CancelEvent -= OnNoButtonPressed;
        }

        public void OnYesButtonPressed()
        {
            YesButtonPressed?.Invoke();
        }

        public void OnNoButtonPressed()
        {
            NoButtonPressed?.Invoke();
        }
    }
}