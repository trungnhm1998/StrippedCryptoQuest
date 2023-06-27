using Core.Runtime.Events.ScriptableObjects;
using CryptoQuest.Characters.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI
{
    public class UIDialog : MonoBehaviour, IDialog
    {
        [Header("Events received")]
        [SerializeField] private VoidEventChannelSO _showDialogEvent;

        [SerializeField] private VoidEventChannelSO _hideDialogEvent;
        [SerializeField] private DialogEventScriptableObject _dialogEvent;

        [Space]
        [SerializeField] private GameObject _content;

        [SerializeField] private TextMeshProUGUI _text;

        public bool IsShown => _content.activeSelf;

        public GameObject Content
        {
            get => _content;
            set => _content = value;
        }

        public VoidEventChannelSO ShowDialogEvent
        {
            get => _showDialogEvent;
            set => _showDialogEvent = value;
        }

        public VoidEventChannelSO HideDialogEvent
        {
            get => _hideDialogEvent;
            set => _hideDialogEvent = value;
        }

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        public void Show()
        {
            _content.SetActive(true);
        }

        public void Hide()
        {
            _content.SetActive(false);
        }

        public void SetText(LocalizedString localizedString)
        {
            _text.text = localizedString.GetLocalizedString();
        }

        public void RegisterEvents()
        {
            _showDialogEvent.EventRaised += Show;
            _hideDialogEvent.EventRaised += Hide;
            _dialogEvent.ShowEvent += SetText;
        }

        public void UnregisterEvents()
        {
            _showDialogEvent.EventRaised -= Show;
            _hideDialogEvent.EventRaised -= Hide;
            _dialogEvent.ShowEvent -= SetText;
        }
    }
}