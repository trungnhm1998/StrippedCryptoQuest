using Core.Runtime.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI
{
    public class UIDialog : MonoBehaviour, IDialog, IEvents
    {
        [SerializeField] private VoidEventChannelSO _showDialogEvent;
        [SerializeField] private GameObject _content;

        public bool IsShown { get => _content.activeSelf; }
        public GameObject Content
        {
            get => _content; set => _content = value;
        }

        public VoidEventChannelSO ShowDialogEvent
        {
            get => _showDialogEvent;
            set => _showDialogEvent = value;
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

        public void RegisterEvents()
        {
            _showDialogEvent.EventRaised += Show;
        }

        public void UnregisterEvents()
        {
            _showDialogEvent.EventRaised -= Show;
        }
    }
}