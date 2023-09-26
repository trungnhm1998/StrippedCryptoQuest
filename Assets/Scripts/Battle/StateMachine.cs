using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.Battle.UI.SelectCommand;
using CryptoQuest.Battle.UI.StartBattle;
using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public interface IState
    {
        void OnEnter(StateMachine stateMachine);
        void OnExit(StateMachine stateMachine);
    }

    public class StateMachine : MonoBehaviour
    {
        [field: SerializeField] public BattleInputSO BattleInput { get; private set; }
        [SerializeField] private BattleManager _battleManager;

        #region Context

        [field: SerializeField] public GameObject BattleUI { get; private set; }
        [field: SerializeField] public UIIntroBattle IntroUI { get; private set; }
        [field: SerializeField] public UISelectCommand CommandUI { get; private set; }
        [field: SerializeField] public CommandDetailPresenter CommandDetailPresenter { get; private set; }
        [field: SerializeField] public EnemiesPresenter EnemiesPresenter { get; private set; }

        #endregion

        private StateFactory _stateFactory;
        public StateFactory Factory => _stateFactory;

        private IState _currentState;

        private void Awake()
        {
            _stateFactory = new StateFactory(this);
        }

        private void OnEnable()
        {
            _battleManager.Initialized += StartBattle;
        }

        private void OnDisable()
        {
            _battleManager.Initialized -= StartBattle;
        }

        private void StartBattle()
        {
            ChangeState(_stateFactory.CreateIntro());
        }

        public void ChangeState(IState state)
        {
            Debug.Log($"BattleStateMachine: ChangeState {state.GetType().Name}");
            _currentState?.OnExit(this);
            _currentState = state;
            _currentState.OnEnter(this);
        }
    }
}