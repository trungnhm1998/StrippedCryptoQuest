using System;
using Core.Runtime.Events.ScriptableObjects;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI
{
    public class UITitle : MonoBehaviour
    {
        [SerializeField] private InputMediator _inputMediator;
        [SerializeField] private Button _startGameButton;

        [Header("Listen on")]
        [SerializeField] private VoidEventChannelSO _sceneLoaded;

        private void OnEnable()
        {
            _sceneLoaded.EventRaised += SceneLoadedEvent_Raised;
        }
        
        private void OnDisable()
        {
            _sceneLoaded.EventRaised -= SceneLoadedEvent_Raised;
        }

        private void SceneLoadedEvent_Raised()
        {
            _inputMediator.EnableMenuInput();
            _startGameButton.Select();
        }

        public void StartGameButtonClicked()
        {
            Debug.Log("Start Game Button Clicked");
        }
    }
}