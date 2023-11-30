using System.Collections;
using CryptoQuest.Gameplay;
using CryptoQuest.Input;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.UI;
using UnityEngine;

namespace CryptoQuest.Battle.PostBattleHandlers
{
    /// <summary>
    /// This should live on gameplay manager and wait until battle scene finished clean up
    /// 
    /// Reward if won or teleport to closest town if lost after battle scene unloaded then will fade out
    /// </summary>

    // TODO?: A little DRY here since concrete class have to drag all the field with the same object
    // Can refactor this to a separate component so other handler can Unload battle
    public abstract class PostBattleManager : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private FadeConfigSO _fadeController;
        [SerializeField] private SceneScriptableObject _battleSceneSO;
        [SerializeField] private GameStateSO _gameState;
        protected SceneScriptableObject BattleSceneSO => _battleSceneSO;
        [SerializeField] private UnloadSceneEventChannelSO _unloadSceneEvent;

        protected void UnloadBattleScene() => _unloadSceneEvent.RequestUnload(_battleSceneSO);

        protected void FinishPresentationAndEnableInput()
        {
            StartCoroutine(CoFinishPresentation());
        }

        private IEnumerator CoFinishPresentation()
        {
            _fadeController.OnFadeOut();
            yield return new WaitForSeconds(_fadeController.Duration);
            OnFadeOut();
            // TODO: After battle there could be a cutscene, so we should enable correct input
            _gameState.UpdateGameState(EGameState.Field);
            _inputMediator.EnableMapGameplayInput();
        }

        protected virtual void OnFadeOut() { }
    }
}