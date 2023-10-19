using System.Collections;
using CryptoQuest.Battle.Events;
using CryptoQuest.Input;
using IndiGames.Core.UI;
using UnityEngine;

namespace CryptoQuest.Battle
{
    /// <summary>
    /// This should live on battle scene so we could release battle assets
    /// </summary>
    public class BattleUnloader : MonoBehaviour
    {
        [SerializeField] private BattleInput _battleInput;
        [SerializeField] private FadeConfigSO _fadeController;
        [SerializeField] private BattleStateMachine _battleStateMachine;
        [SerializeField] private float _showDialogDelay = 1f;

        public IEnumerator FadeInAndUnloadBattle()
        {
            _battleInput.DisableAllInput();
            yield return new WaitForSeconds(_showDialogDelay);
            _fadeController.OnFadeIn();
            yield return new WaitForSeconds(_fadeController.Duration);
            UnloadBattle();
        }

        private void UnloadBattle()
        {
            BattleEventBus.RaiseEvent(new UnloadingEvent());
            _battleStateMachine.Unload();
            BattleEventBus.RaiseEvent(new BattleCleanUpFinishedEvent());
        }
    }
}