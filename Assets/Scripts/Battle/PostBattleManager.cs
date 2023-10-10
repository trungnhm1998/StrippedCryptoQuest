using System.Collections;
using CryptoQuest.Input;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.UI;
using UnityEngine;

namespace CryptoQuest.Battle
{
    /// <summary>
    /// This should live on gameplay manager and wait until battle scene finished clean up
    /// 
    /// Reward if won or teleport to closest town if lost after battle scene unloaded then will fade out
    /// </summary>
    public abstract class PostBattleManager : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private FadeConfigSO _fadeController;
        [SerializeField] private SceneScriptableObject _battleSceneSO;
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
            _inputMediator.EnableMapGameplayInput();
        }

        protected virtual void OnFadeOut() { }
    }
}