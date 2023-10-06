using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
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
        [SerializeField] private BattleContext _battleContext;
        private readonly RoundEndedEvent _roundEndedEvent = new();

        public void ExecuteCharacterCommands(IEnumerable<Components.Character> characters)
        {
            StartCoroutine(CoPresentation(characters));
        }

        private IEnumerator CoPresentation(IEnumerable<Components.Character> characters)
        {
            ChangeAllEnemiesOpacity(0.5f);

            foreach (var character in characters)
            {
                character.TryGetComponent(out CommandExecutor commandExecutor);
                yield return commandExecutor.PreTurn();
                character.UpdateTarget(_battleContext);
                yield return commandExecutor.ExecuteCommand();
                yield return commandExecutor.PostTurn();
                if (CanContinueRound() == false) break;
            }

            ChangeAllEnemiesOpacity(1f);
            BattleEventBus.RaiseEvent(_roundEndedEvent); // Need to be raise so guard tag can be remove
            OnRoundEndedCheck();
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
            }
            else if (IsLost())
            {
                Debug.Log("Battle Lost");
                Lost?.Invoke();
            }
        }

        private bool CanContinueRound() => IsWon() == false && IsLost() == false;

        private bool IsWon() =>
            _battleContext.Enemies.All(enemy => !enemy.IsValid()); // TODO: Cache and reset on round started

        // TODO: Cache and reset on round started
        private static bool IsLost()
        {
            var playerParty = ServiceProvider.GetService<IPartyController>();
            return playerParty.Slots.Any(slot => slot.IsValid());
        }

        private void ChangeAllEnemiesOpacity(float f)
        {
            foreach (var enemy in _battleContext.Enemies.Where(enemy => enemy.IsValid()))
            {
                enemy.SetAlpha(f);
            }
        }
    }
}