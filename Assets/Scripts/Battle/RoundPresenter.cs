using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.UI.Logs;
using CryptoQuest.Character.Ability;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle
{
    /// <summary>
    /// I need a mono for coroutine, or using a tween library but it still using mono under the hood
    /// </summary>
    public class RoundPresenter : MonoBehaviour
    {
        public event Action Lost;
        public event Action Won;
        [SerializeField] private LogPresenter _logPresenter;
        [SerializeField] private BattleContext _battleContext;
        [SerializeField] private RetreatAbility _retreatAbility;
        private readonly RoundEndedEvent _roundEndedEvent = new();
        public event Action EndBattle;
        private bool _isBattleEnded;

        private TinyMessageSubscriptionToken _highlightEnemyToken;
        private TinyMessageSubscriptionToken _resetHighlightEnemyToken;

        private void OnEnable()
        {
            _retreatAbility.RetreatedEvent += OnEndBattle;
            _highlightEnemyToken = BattleEventBus.SubscribeEvent<HighlightEnemyEvent>(HightlightEnemy);
            _resetHighlightEnemyToken = BattleEventBus.SubscribeEvent<ResetHighlightEnemyEvent>(ResetHightlightEnemy);
        }

        private void OnDisable()
        {
            _retreatAbility.RetreatedEvent -= OnEndBattle;
            BattleEventBus.UnsubscribeEvent(_highlightEnemyToken);
            BattleEventBus.UnsubscribeEvent(_resetHighlightEnemyToken);
        }

        public void ExecuteCharacterCommands(IEnumerable<Components.Character> characters)
        {
            StartCoroutine(CoPresentation(characters));
        }

        private IEnumerator CoPresentation(IEnumerable<Components.Character> characters)
        {
            ChangeAllEnemiesOpacity(1f);

            foreach (var character in characters)
            {
                _logPresenter.Clear();
                character.TryGetComponent(out CommandExecutor commandExecutor);
                commandExecutor.PreTurn();
                yield return new WaitUntil(() => _logPresenter.Finished); // TODO: I think this is a bad code
                character.UpdateTarget(_battleContext);
                yield return commandExecutor.ExecuteCommand();
                yield return new WaitUntil(() => _logPresenter.Finished); // TODO: I think this is a bad code
                yield return commandExecutor.PostTurn();
                yield return new WaitUntil(() => _logPresenter.Finished); // TODO: I think this is a bad code
                if (CanContinueRound() == false) break;
            }

            yield return new WaitUntil(() => _logPresenter.Finished);
            _logPresenter.HideAndClear();
            BattleEventBus.RaiseEvent(_roundEndedEvent); // Need to be raise so guard tag can be remove
            OnRoundEndedCheck();
        }

        private bool CanContinueRound()
        {
            _battleContext.UpdateBattleContext();
            return !IsWon() && !IsLost() && !_isBattleEnded;
        }

        /// <summary>
        /// Always check won before lost because of skill that killed enemy by sacrificing self
        /// </summary>
        private void OnRoundEndedCheck()
        {
            if (IsWon())
            {
                Debug.Log("Battle Won");
                Won?.Invoke();
                return;
            }

            if (IsLost())
            {
                Debug.Log("Battle Lost");
                Lost?.Invoke();
            }
        }

        private bool IsWon() => _battleContext.IsAllEnemiesDead;
        
        private bool IsLost() => _battleContext.IsAllHeroesDead;

        /// <summary>
        /// When battle ended with retreat player will not have reward 
        /// but still stay in the battle field scene
        /// TODO: steal ability/behaviour can listen to this event and add stealed loot 
        /// </summary>
        private void OnEndBattle()
        {
            _isBattleEnded = true;
            EndBattle?.Invoke();
        }

        private void ChangeAllEnemiesOpacity(float f)
        {
            foreach (var enemy in _battleContext.Enemies.Where(enemy => enemy.IsValidAndAlive()))
            {
                enemy.SetAlpha(f);
            }
        }

        private void HightlightEnemy(HighlightEnemyEvent eventObject)
        {
            ChangeAllEnemiesOpacity(0.5f);
            eventObject.Enemy.SetAlpha(1f);
        }

        private void ResetHightlightEnemy(ResetHighlightEnemyEvent eventObject)
        {
            ChangeAllEnemiesOpacity(1f);
        }
    }
}