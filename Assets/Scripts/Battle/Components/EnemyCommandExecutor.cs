using System.Collections;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Events;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class EnemyCommandExecutor : CommandExecutor
    {
        [SerializeField] private EnemyBehaviour _enemyBehaviour;

        private HighlightEnemyEvent _highlightEvent = new HighlightEnemyEvent();
        private ResetHighlightEnemyEvent _resetHighlightEvent = new ResetHighlightEnemyEvent();

        private void Start()
        {
            _highlightEvent.Enemy = _enemyBehaviour;
        }

        protected override IEnumerator OnPreExecuteCommand()
        {
            Command = new NormalAttackCommand(Character, Character.Targeting.Target);
            BattleEventBus.RaiseEvent<HighlightEnemyEvent>(_highlightEvent);
            yield return base.OnPreExecuteCommand();
        }

        protected override IEnumerator OnPostExecuteCommand()
        {
            BattleEventBus.RaiseEvent<ResetHighlightEnemyEvent>(_resetHighlightEvent);
            yield return base.OnPostExecuteCommand();
        }
    }
}