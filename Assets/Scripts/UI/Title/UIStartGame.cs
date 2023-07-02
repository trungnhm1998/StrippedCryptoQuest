using System.Collections;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.UI.Title
{
    public class UIStartGame : MonoBehaviour
    {
        [SerializeField] private Button _startGame;
        [SerializeField] private InputMediatorSO _inputMediator;

        public UnityAction StartButtonPressed;

        private void OnEnable()
        {
            _inputMediator.MenuConfirmPressed += OnStartButtonPressed;
        }

        private void OnDisable()
        {
            _inputMediator.MenuConfirmPressed -= OnStartButtonPressed;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f); // event system bug workaround
            _startGame.Select();
        }

        public void OnStartButtonPressed()
        {
            StartButtonPressed?.Invoke();
        }
    }
}