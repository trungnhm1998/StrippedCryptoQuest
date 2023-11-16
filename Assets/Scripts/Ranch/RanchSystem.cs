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

        [SerializeField] private RanchController ranchController;
        [SerializeField] private RanchInputManager ranchInputManager;
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
            ranchController.gameObject.SetActive(true);
            RanchOpenedEvent?.Invoke();
            EnableFarmInput();
        }

        private void CloseFarmRequested()
        {
            DisableFarmInput();
            RanchClosedEvent?.Invoke();
            ranchController.gameObject.SetActive(false);
        }

        private void EnableFarmInput()
        {
            ranchInputManager.EnableInput();
        }

        private void DisableFarmInput()
        {
            ranchInputManager.DisableInput();
        }
    }
}