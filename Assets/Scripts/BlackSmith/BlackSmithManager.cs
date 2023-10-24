using System;
using System.Collections;
using CryptoQuest.BlackSmith.ScriptableObjects;
using CryptoQuest.BlackSmith.StateMachine;
using CryptoQuest.Input;
using CryptoQuest.Menu;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith
{
    public class BlackSmithManager : MonoBehaviour
    {
        [SerializeField] private GameObject _stateController;
        [SerializeField] private BlackSmithInputManager _blackSmithInput;
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;
        [SerializeField] private ShowBlackSmithEventChannelSO _openBlackSmithEvent;
        [SerializeField] private BlackSmithStateController _blacksmithController;

        [Header("Unity Events")]
        [SerializeField] private UnityEvent _blackSmithOpenedEvent;
        [SerializeField] private UnityEvent _blackSmithClosedEvent;

        private void OnEnable()
        {
            _openBlackSmithEvent.EventRaised += OpenBlackSmith;
            _blacksmithController.CloseStateEvent += CloseBlackSmith;
        }

        private void OnDisable()
        {
            _openBlackSmithEvent.EventRaised -= OpenBlackSmith;
            _blacksmithController.CloseStateEvent -= CloseBlackSmith;
        }

        private void OpenBlackSmith()
        {
            EnableBlackSmithInput();
            _stateController.SetActive(true);
            _blackSmithOpenedEvent.Invoke();
        }

        private void CloseBlackSmith()
        {
            DisableBlackSmithInput();
            _stateController.SetActive(false);
            _blackSmithClosedEvent.Invoke();
        }

        private void EnableBlackSmithInput()
        {
            _blackSmithInput.EnableInput();
        }

        private void DisableBlackSmithInput()
        {
            _blackSmithInput.DisableInput();
        }
    }
}
