using CryptoQuest.Battle;
using CryptoQuest.Gameplay;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Map
{
    /// <summary>
    /// Wrapper GoTo load map event to prevent load map and battle at the same time
    /// </summary>
    public class PreventLoadMapAndBattle : MonoBehaviour
    {
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private BattleBus _battleBus;
        [Header("Listening to")]
        [SerializeField] private LoadSceneEventChannelSO _loadSceneOnGoToEventChannel;
        [Header("Raise")]
        [SerializeField] private LoadSceneEventChannelSO _loadMapEventChannel;

        private void OnEnable()
        {
            _loadSceneOnGoToEventChannel.LoadingRequested += OnRequestLoad;
        }

        private void OnDisable()
        {
            _loadSceneOnGoToEventChannel.LoadingRequested -= OnRequestLoad;
        }

        private void OnRequestLoad(SceneScriptableObject sceneSO)
        {
            if (_gameState.CurrentGameState == EGameState.Battle)
            {
                Debug.LogWarning($"Load map request ignored because battle is in progress");
                return;
            }
            
            // If load map happend first then disable current encounter so battle wont be loaded
            _battleBus.CurrentEncounter = null;
            _loadMapEventChannel.RequestLoad(sceneSO);
        }
    }
}