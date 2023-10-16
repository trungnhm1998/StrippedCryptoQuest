using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Events;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class EnemyCommandExecutor : CommandExecutor
    {
        [SerializeField] private EnemyBehaviour _enemyBehaviour;

        private readonly HighlightEnemyEvent _highlightEvent = new();
        private readonly ResetHighlightEnemyEvent _resetHighlightEvent = new();

        private void Start()
        {
            _highlightEvent.Enemy = _enemyBehaviour;
        }

        protected override void OnPreExecuteCommand()
        {
            BattleEventBus.RaiseEvent(_highlightEvent);
        }

        protected override void OnPostExecuteCommand()
        {
            base.OnPostExecuteCommand();
            BattleEventBus.RaiseEvent(_resetHighlightEvent);
        }
    }
}