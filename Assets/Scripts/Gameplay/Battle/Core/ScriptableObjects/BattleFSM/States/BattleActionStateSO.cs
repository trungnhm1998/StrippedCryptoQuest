using System.Collections;
using CryptoQuest.FSM;
using CryptoQuest.Gameplay.Battle.Core.Commands;
using CryptoQuest.Gameplay.Battle.Core.Commands.BattleCommands;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.BattleFSM.States
{
    [CreateAssetMenu(fileName = "BattleActionStateSO", menuName = "Gameplay/Battle/FSM/States/Battle Action State")]
    public class BattleActionStateSO : BattleStateSO
    {
        [SerializeField] private VoidEventChannelSO _endActionPhaseEventChannel;
        [SerializeField] private VoidEventChannelSO _showNextMarkEventChannel;
        [SerializeField] private VoidEventChannelSO _doneShowDialogEvent;

        private BattleCommandHandler _commandHandler;

        private Coroutine _unitActionCoroutine;

        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            _unitActionCoroutine = stateMachine.StartCoroutine(PerformBattleUnitsAction(stateMachine));
            _commandHandler = BattleManager.BattleCommandHandler;
        }

        private IEnumerator PerformBattleUnitsAction(BaseStateMachine stateMachine)
        {
            foreach (var unit in BattleManager.GetActionOrderList())
            {
                yield return unit.Execute();
            }

            _commandHandler.ExecuteCommand();
            yield return new WaitUntil(() => _commandHandler.IsQueueEmpty);

            _endActionPhaseEventChannel.RaiseEvent();
            stateMachine.SetCurrentState(_nextState);
        }

        public override void OnExitState(BaseStateMachine stateMachine)
        {
            base.OnExitState(stateMachine);
            if (_unitActionCoroutine == null) return;
            stateMachine.StopCoroutine(_unitActionCoroutine);
        }
    }
}