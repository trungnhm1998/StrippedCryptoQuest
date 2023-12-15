using CryptoQuest.Events.UI;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Merchant
{
    public abstract class MerchantSystemBase : MonoBehaviour
    {
        [SerializeField] private ShowWalletEventChannelSO _showCurrenciesPanel;
        [SerializeField] private GameObject _canvas;
        protected GameObject Canvas => _canvas;

        [field: SerializeField, Header("Listening to")] public VoidEventChannelSO OpenSystemEvent { get; private set; }

        [field: SerializeField] public VoidEventChannelSO CloseSystemEvent { get; private set; }

        protected virtual void Start()
        {
            Canvas.gameObject.SetActive(false);
        }

        private bool _hasClosed;

        private void OnEnable()
        {
            OpenSystemEvent.EventRaised += InitSystem;
            CloseSystemEvent.EventRaised += ExitSystem;

            _hasClosed = false;
        }

        private void OnDisable()
        {
            OpenSystemEvent.EventRaised -= InitSystem;
            CloseSystemEvent.EventRaised -= ExitSystem;
            ExitSystem();
        }

        private void InitSystem()
        {
            if (_showCurrenciesPanel) _showCurrenciesPanel.Show();
            _hasClosed = false;
            _canvas.SetActive(true);
            OnInit();
        }

        private void ExitSystem()
        {
            if (_showCurrenciesPanel) _showCurrenciesPanel.Hide();
            if (_hasClosed) return;
            _hasClosed = true;
            _canvas.SetActive(false);
            OnExit();
        }

        protected abstract void OnInit();
        protected abstract void OnExit();
    }
}