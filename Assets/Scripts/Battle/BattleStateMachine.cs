using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.States;
using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.Battle.UI.SelectCommand;
using CryptoQuest.Battle.UI.StartBattle;
using CryptoQuest.Input;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public interface IState
    {
        void OnEnter(BattleStateMachine stateMachine);
        void OnExit(BattleStateMachine stateMachine);
        void OnDestroy(BattleStateMachine battleStateMachine);
    }
    
    public class BattleStateMachine : MonoBehaviour
    {
        [field: SerializeField] public BattleInputSO BattleInput { get; private set; }

        #region State Context

        [field: SerializeField] public GameObject BattleUI { get; private set; }
        [field: SerializeField] public UIIntroBattle IntroUI { get; private set; }
        [field: SerializeField] public UISelectCommand CommandUI { get; private set; }
        [field: SerializeField] public CommandDetailPresenter CommandDetailPresenter { get; private set; }
        [field: SerializeField] public SpiralConfigSO Spiral { get; private set; }
        [field: SerializeField] public VoidEventChannelSO SceneLoadedEvent { get; private set; }
        [field: SerializeField] public BattleEventSO BattleEndedEvent { get; private set; }

        #endregion

        private IState _currentState;

        private void Awake()
        {
            SceneLoadedEvent.EventRaised += GotoLoadingState;
        }

        private void OnDestroy()
        {
            SceneLoadedEvent.EventRaised -= GotoLoadingState;
            _currentState.OnDestroy(this);
        }

        private void GotoLoadingState()
        {
            ChangeState(new Loading());
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