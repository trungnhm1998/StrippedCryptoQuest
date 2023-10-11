using System;
using System.Collections;
using CryptoQuest.BlackSmith.ScriptableObjects;
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
        [SerializeField] private ShowBlackSmithEventChannelSO _openBlackSmithEvent;
        [SerializeField] private UnityEvent _blackSmithOpenedEvent;

        private void OnEnable()
        {
            _openBlackSmithEvent.EventRaised += OpenBlackSmith;
        }

        private void OnDisable()
        {
            _openBlackSmithEvent.EventRaised -= OpenBlackSmith;
        }

        private void OpenBlackSmith()
        {
            EnableBlackSmithInput();
            _blackSmithOpenedEvent.Invoke();
        }

        private void EnableBlackSmithInput()
        {
            _blackSmithInput.EnableInput();
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
