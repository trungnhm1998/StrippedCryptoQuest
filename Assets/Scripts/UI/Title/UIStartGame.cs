using CryptoQuest.System;
using CryptoQuest.System.SaveSystem;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.UI.Title
{
    public class UIStartGame : MonoBehaviour
    {
        [field: SerializeField] public Button StartGameBtn { get; private set; }
        [SerializeField] private VoidEventChannelSO _startGameEventChannel;
        public UnityAction StartButtonPressed;

        public void InitStartGameUI()
        {
            StartGameBtn.Select();
        }

        public void OnStartButtonPressed()
        {
            StartButtonPressed?.Invoke();
        }

        public void StartGame()
        {
            _startGameEventChannel.RaiseEvent();
        }
    }
}