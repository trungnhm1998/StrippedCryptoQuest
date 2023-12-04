using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Merchant
{
    public abstract class MerchantSystemBase : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;

        [Header("Listening to")]
        [field: SerializeField] public VoidEventChannelSO OpenSystemEvent;

        [field: SerializeField] public VoidEventChannelSO CloseSystemEvent;

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
            CloseSystemEvent.EventRaised += ExitSystem;
            ExitSystem();
        }

        private void InitSystem()
        {
            _canvas.SetActive(true);
            OnInit();
        }

        private void ExitSystem()
        {
            if (_hasClosed) return;
            _hasClosed = true;
            _canvas.SetActive(false);
            OnExit();
        }

        protected abstract void OnInit();
        protected abstract void OnExit();
    }
}