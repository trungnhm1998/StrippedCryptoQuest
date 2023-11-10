using UnityEngine;
using CryptoQuest.ChangeClass.ScriptableObjects;
using UnityEngine.UI;
using UnityEngine.Events;
using CryptoQuest.Input;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassManager : MonoBehaviour
    {
        public UnityAction EnterChangeClassStateEvent;
        public UnityAction<bool> IsOpenChangeClassState;
        [SerializeField] private ShowChangeClassEventChannelSO _showChangeClassEvent;
        [SerializeField] private ChangeClassDialogController _dialogController;
        [SerializeField] private ChangeClassStateController _stateController;
        [SerializeField] private MerchantsInputManager _input;
        [SerializeField] private GameObject _changeClassPanel;
        [SerializeField] private Button _defaultButton;


        private void OnEnable()
        {
            _showChangeClassEvent.EventRaised += ShowChangeClass;
            _stateController.ExitStateEvent += HideChangeClass;
            _defaultButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _showChangeClassEvent.EventRaised -= ShowChangeClass;
            _stateController.ExitStateEvent -= HideChangeClass;
            _defaultButton.onClick.RemoveListener(OnButtonClicked);
        }

        private void ShowChangeClass()
        {
            IsOpenChangeClassState?.Invoke(true);
            _dialogController.ShowChangeClassDialog();
            _defaultButton.Select();
            _input.EnableInput();
            _changeClassPanel.SetActive(true);
        }

        private void HideChangeClass()
        {
            IsOpenChangeClassState?.Invoke(false);
            _changeClassPanel.SetActive(false);
            _dialogController.HideChangeClassDialog();
            _input.DisableInput();
        }

        private void OnButtonClicked()
        {
            EnterChangeClassStateEvent?.Invoke();
        }
    }
}
