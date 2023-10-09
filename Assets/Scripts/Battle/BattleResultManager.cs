using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward;
using CryptoQuest.Input;
using IndiGames.Core.UI;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle
{
    /// <summary>
    /// This should live on battle scene so we could release battle assets
    /// </summary>
    public class BattleResultManager : MonoBehaviour
    {
        [SerializeField] private BattleContext _battleContext;
        [SerializeField] private FadeConfigSO _fadeController;
        [SerializeField] private BattleStateMachine _battleStateMachine;
        [SerializeField] private RoundPresenter _roundPresenter;
        [SerializeField] private float _showDialogDelay = 1f;
        private TinyMessageSubscriptionToken _forceWonEventToken;
        private TinyMessageSubscriptionToken _forceLoseEventToken;

        private void Awake()
        {
            _roundPresenter.Lost += OnLost;
            _roundPresenter.Won += OnWon;
            _roundPresenter.EndBattle += OnEndBattle;

            _forceWonEventToken = BattleEventBus.SubscribeEvent<ForceWinBattleEvent>(ForceWon);
            _forceLoseEventToken = BattleEventBus.SubscribeEvent<ForceLoseBattleEvent>(ForceLose);
        }

        private void OnDestroy()
        {
            _roundPresenter.Lost -= OnLost;
            _roundPresenter.Won -= OnWon;
            _roundPresenter.EndBattle -= OnEndBattle;

            BattleEventBus.UnsubscribeEvent(_forceWonEventToken);
            BattleEventBus.UnsubscribeEvent(_forceLoseEventToken);
        }

        private void ForceWon(ForceWinBattleEvent _) => StartCoroutine(CoOnPresentWon());
        private void ForceLose(ForceLoseBattleEvent _) => StartCoroutine(CoOnPresentLost());
        private void OnWon() => StartCoroutine(CoOnPresentWon());
        private void OnLost() => StartCoroutine(CoOnPresentLost());
        private void OnEndBattle() => StartCoroutine(CoOnPresentEnd());

        private IEnumerator CoOnPresentWon()
        {
            Debug.Log("Show dialog Battle Won");
            yield return FadeInAndUnloadBattle();
            var loots = new List<LootInfo>();
            foreach (var enemy in _battleContext.Enemies.Where(enemy => enemy.Spec.IsValid()))
            {
                loots.AddRange(enemy.GetLoots());
            }

            loots = RewardManager.CloneAndMergeLoots(loots);
            BattleEventBus.RaiseEvent(new BattleWonEvent()
            {
                Battlefield = _battleContext.CurrentBattlefield,
                Loots = loots
            });
        }

        private IEnumerator CoOnPresentLost()
        {
            Debug.Log("Show dialog Battle Lost");
            yield return FadeInAndUnloadBattle();
            // TODO: Unload current battle field scene and load last saved scene
            BattleEventBus.RaiseEvent(new BattleLostEvent()
            {
                Battlefield = _battleContext.CurrentBattlefield,
            });
        }
        
        private IEnumerator CoOnPresentEnd()
        {
            Debug.Log("Show dialog Battle End");
            yield return FadeInAndUnloadBattle();
            BattleEventBus.RaiseEvent(new BattleEndedEvent()
            {
                Battlefield = _battleContext.CurrentBattlefield,
            });
        }

        private IEnumerator FadeInAndUnloadBattle()
        {
            BattleInput.instance.DisableAllInput();
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