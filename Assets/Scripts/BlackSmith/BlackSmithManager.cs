using System;
using System.Collections;
using CryptoQuest.BlackSmith.EvolveStates;
using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith
{
    public class BlackSmithManager : MonoBehaviour
    {
        [SerializeField] private GameObject _evolveStateController;
        [SerializeField] private GameObject _upgradeStateController;
        [SerializeField] private BlackSmithInputManager _blackSmithInput;
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;
        [SerializeField] private UnityEvent _inputEnabledEvent;

        private void OnEnable()
        {
            _sceneLoadedEvent.EventRaised += EnableBlackSmithInput;
        }

        private void OnDisable()
        {
            _sceneLoadedEvent.EventRaised -= EnableBlackSmithInput;
        }

        private void EnableBlackSmithInput()
        {
            _blackSmithInput.EnableInput();
            _inputEnabledEvent.Invoke();
        }

        public void EvolveButtonPressed()
        {
            _evolveStateController.SetActive(true);
        }

        public void UpgradeButtonPressed()
        {
            _upgradeStateController.SetActive(true);
        }
    }
}
