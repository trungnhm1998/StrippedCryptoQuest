using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.TrainingBattle.State
{
    public class LostBattleStateBehaviour : BaseStateBehaviour
    {
        private TrainingBattleStateController _stateController;
        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<TrainingBattleStateController>();
            if(!_stateController.IsExitState) return;
            _stateController.ExitStateEvent?.Invoke();
            _stateController.IsExitState = false;
        }

        protected override void OnExit() { }
    }
}