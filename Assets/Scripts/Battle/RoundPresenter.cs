using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.UI.Logs;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle
{
    // TODO: Move logic into RoundPresentation.cs
    public class RoundPresenter : MonoBehaviour
    {
        public event Action Lost;
        public event Action Won;
        [SerializeField] private BattleContext _battleContext;
        [SerializeField] private RetreatAbility _retreatAbility;
        private readonly RoundEndedEvent _roundEndedEvent = new();
        public event Action EndBattle;
        private bool _isBattleEnded;
        private int _roundCount;

        private TinyMessageSubscriptionToken _highlightEnemyToken;
        private TinyMessageSubscriptionToken _resetHighlightEnemyToken;

        private void OnEnable()
        {
            _roundCount = 0;
            _retreatAbility.RetreatedEvent += OnEndBattle;
            _highlightEnemyToken = BattleEventBus.SubscribeEvent<HighlightEnemyEvent>(HighlightEnemy);
            _resetHighlightEnemyToken = BattleEventBus.SubscribeEvent<ResetHighlightEnemyEvent>(ResetHighlightEnemy);
        }

        private void OnDisable()
        {
            _retreatAbility.RetreatedEvent -= OnEndBattle;
            BattleEventBus.UnsubscribeEvent(_highlightEnemyToken);
            BattleEventBus.UnsubscribeEvent(_resetHighlightEnemyToken);
        }

        public void ExecuteCharacterCommands(IEnumerable<Components.Character> characters)
        {
            Debug.Log($"Presenting round [{++_roundCount}]");
            ChangeAllEnemiesOpacity(1f);

            foreach (var character in characters)
            {
                if (character.IsValidAndAlive() == false) continue; // could die because of last turn
                OnTurnStarting(character);
                character.OnTurnStarted();
                character.TryGetComponent(out CommandExecutor commandExecutor);
                character.UpdateTarget(_battleContext);
                OnExecutingCommand(character);
                commandExecutor.ExecuteCommand();
                character.OnTurnEnded();
                if (CanContinueRound() == false) break;
            }

            // yield return new WaitUntil(() => _logPresenter.Finished);
            BattleEventBus.RaiseEvent(_roundEndedEvent); // Need to be raise so guard tag can be remove
            OnCheckRoundResult();
        }

        private static void OnTurnStarting(Components.Character character)
        {
            BattleEventBus.RaiseEvent(new TurnStartedEvent()
            {
                Character = character
            });
        }

        private static void OnExecutingCommand(Components.Character character)
        {
            BattleEventBus.RaiseEvent(new ExecutingCommandEvent()
            {
                Character = character
            });
        }

        private bool CanContinueRound()
        {
            _battleContext.UpdateBattleContext();
            return !IsWon() && !IsLost() && !_isBattleEnded;
        }

        /// <summary>
        /// Need at least one surviving character to win
        /// </summary>
        private void OnCheckRoundResult()
        {
            if (IsLost())
            {
                Debug.Log("Battle Lost");
                Lost?.Invoke();
            }

            if (IsWon())
            {
                Debug.Log("Battle Won");
                Won?.Invoke();
                return;
            }
        }

        private bool IsWon() => _battleContext.IsAllEnemiesDead;

        private bool IsLost() => _battleContext.IsAllHeroesDead;

        /// <summary>
        /// When battle ended with retreat player will not have reward 
        /// but still stay in the battle field scene
        /// TODO: steal ability/behaviour can listen to this event and add stealed loot 
        /// </summary>
        private void OnEndBattle(AbilitySystemBehaviour owner)
        {
            _isBattleEnded = true;
            EndBattle?.Invoke();
        }

        private void ChangeAllEnemiesOpacity(float f)
        {
            foreach (var enemy in _battleContext.Enemies.Where(enemy => enemy.IsValidAndAlive())) enemy.SetAlpha(f);
        }

        private void HighlightEnemy(HighlightEnemyEvent eventObject)
        {
            ChangeAllEnemiesOpacity(0.5f);
            eventObject.Enemy.SetAlpha(1f);
        }

        private void ResetHighlightEnemy(ResetHighlightEnemyEvent eventObject) => ChangeAllEnemiesOpacity(1f);
    }
}