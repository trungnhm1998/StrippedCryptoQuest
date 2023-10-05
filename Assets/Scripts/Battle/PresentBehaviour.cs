using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Battle.Events;
using UnityEngine;

namespace CryptoQuest.Battle
{
    /// <summary>
    /// I need a mono for coroutine, or using a tween library but it still using mono under the hood
    /// </summary>
    public class PresentBehaviour : MonoBehaviour
    {
        [SerializeField] private BattleContext _battleContext;
        private RoundEndedEvent _roundEndedEvent = new RoundEndedEvent();

        public void ExecuteCharacterCommands(IEnumerable<Components.Character> characters)
        {
            StartCoroutine(CoPresentation(characters));
        }

        private IEnumerator CoPresentation(IEnumerable<Components.Character> characters)
        {
            ChangeAllEnemiesOpacity(0.5f);

            foreach (var character in characters)
            {
                yield return character.PreTurn();
                character.UpdateTarget(_battleContext);
                yield return character.ExecuteCommand();
                yield return character.PostTurn();
            }

            ChangeAllEnemiesOpacity(1f);
            BattleEventBus.RaiseEvent<RoundEndedEvent>(_roundEndedEvent);
        }

        private void ChangeAllEnemiesOpacity(float f)
        {
            foreach (var enemy in _battleContext.Enemies)
            {
                if (enemy.IsValid()) enemy.SetAlpha(f);
            }
        }
    }
}