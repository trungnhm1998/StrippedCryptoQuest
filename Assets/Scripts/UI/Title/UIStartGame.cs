using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SaveSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.UI.Title
{
    public class UIStartGame : MonoBehaviour
    {
        [field: SerializeField] public Button StartGameBtn { get; private set; }
        [SerializeField] private VoidEventChannelSO _startGameEventChannel;
        [SerializeField] private SaveSystemSO _saveSystemSo;
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

        public bool IsPlayerNameExist()
        {
            return !string.IsNullOrEmpty(_saveSystemSo.PlayerName);
        }
    }
}