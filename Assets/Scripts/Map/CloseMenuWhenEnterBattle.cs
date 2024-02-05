using CryptoQuest.Battle;
using CryptoQuest.Gameplay;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Map
{
    /// <summary>
    /// Prevent open menu when battle is in progress
    /// </summary>
    public class CloseMenuWhenEnterBattle : MonoBehaviour
    {
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private VoidEventChannelSO _forceCloseMainMenuEvent;

        private void OnEnable()
        {
            BattleLoader.PreLoadBattle += CheckAndCloseMenu;
        }

        private void OnDisable()
        {
            BattleLoader.PreLoadBattle -= CheckAndCloseMenu;
        }

        private void CheckAndCloseMenu()
        {
            if (_gameState.CurrentGameState != EGameState.Menu)
                return;

            _forceCloseMainMenuEvent.RaiseEvent();
        }
    }
}