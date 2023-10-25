using CryptoQuest.Battle.Events;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class EnemyHighlighter : MonoBehaviour
    {
        private EnemyBehaviour _enemyBehaviour;

        private readonly HighlightEnemyEvent _highlightEvent = new();
        private readonly ResetHighlightEnemyEvent _resetHighlightEvent = new();
        private CommandExecutor _commandExecutor;

        private void Awake()
        {
            _enemyBehaviour = GetComponent<EnemyBehaviour>();
            _commandExecutor = GetComponent<CommandExecutor>();
            _highlightEvent.Enemy = _enemyBehaviour;

            _commandExecutor.PreExecuteCommand += OnHighlight;
            _commandExecutor.PostExecuteCommand += OnResetHighlight;
        }

        private void OnDestroy()
        {
            _commandExecutor.PreExecuteCommand -= OnHighlight;
            _commandExecutor.PostExecuteCommand -= OnResetHighlight;
        }

        private void OnHighlight() => BattleEventBus.RaiseEvent(_highlightEvent);

        private void OnResetHighlight() => BattleEventBus.RaiseEvent(_resetHighlightEvent);
    }
}