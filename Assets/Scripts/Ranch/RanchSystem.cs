using CryptoQuest.Input;
using CryptoQuest.Ranch.ScriptableObject;
using CryptoQuest.Ranch.State;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Ranch
{
    public class RanchSystem : MonoBehaviour
    {
        public event UnityAction RanchOpenedEvent;
        public event UnityAction RanchClosedEvent;

        [SerializeField] private RanchController _ranchController;
        [SerializeField] private MerchantsInputManager _ranchInputManager;
        [SerializeField] private RanchStateController _ranchStateController;

        [Header("Listening on Channels")]
        [SerializeField] private ShowRanchEventChannelSO _showRanchEventChannel;

        private void OnEnable()
        {
            _showRanchEventChannel.EventRaised += ShowFarmRequested;
            _ranchStateController.ExitStateEvent += CloseFarmRequested;
        }

        private void OnDisable()
        {
            _showRanchEventChannel.EventRaised -= ShowFarmRequested;
            _ranchStateController.ExitStateEvent -= CloseFarmRequested;
        }

        private void ShowFarmRequested()
        {
            _ranchController.gameObject.SetActive(true);
            RanchOpenedEvent?.Invoke();
            EnableFarmInput();
        }

        private void CloseFarmRequested()
        {
            DisableFarmInput();
            RanchClosedEvent?.Invoke();
            _ranchController.gameObject.SetActive(false);
        }

        private void EnableFarmInput()
        {
            _ranchInputManager.EnableInput();
        }

        private void DisableFarmInput()
        {
            _ranchInputManager.DisableInput();
        }
    }
}