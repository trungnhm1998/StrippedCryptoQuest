using System.Collections;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.UI;
using UnityEngine;

namespace CryptoQuest.Battle.PostBattleHandlers
{
    public class BattleUnloadHandler : MonoBehaviour
    {
        [SerializeField] private FadeConfigSO _fadeController;
        [SerializeField] private SceneScriptableObject _battleSceneSO;
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private UnloadSceneEventChannelSO _unloadSceneEvent;

        public IEnumerator UnloadBattle()
        {
            _unloadSceneEvent.RequestUnload(_battleSceneSO);
            _fadeController.OnFadeOut();
            yield return new WaitForSeconds(_fadeController.Duration);
            _gameState.UpdateGameState(EGameState.Field);
            BattleEventBus.RaiseEvent(new UnloadedBattle());
        }
    }
}