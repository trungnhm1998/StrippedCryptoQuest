using System.Collections.Generic;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.ResultStates;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public enum EBattleResult
    {
        None = 0, // Battle dont have result yet
        Won = 1,
        Lost = 2,
        Retreated = 3
    }

    public class BattleResultManager : MonoBehaviour
    {
        [SerializeField] private BattleStateMachine _stateMachine;

        private IResultState[] _resultStates;
        private IState _currentResultState;
        private Dictionary<EBattleResult, IResultState> _resultStatesDict = new();

        private TinyMessageSubscriptionToken _setResultStateToken;
        private TinyMessageSubscriptionToken _changeResultStateToken;

        private void Awake()
        {
            _resultStates = GetComponents<IResultState>();
            foreach (var resultState in _resultStates)
            {
                _resultStatesDict.TryAdd(resultState.Result, resultState);
            }
            SetBattleResult(EBattleResult.None);
        }

        private void OnEnable()
        {
            _setResultStateToken = BattleEventBus.SubscribeEvent<SetResultStateEvent>(
                ctx => SetBattleResult(ctx.Result));
            _changeResultStateToken = BattleEventBus.SubscribeEvent<ChangeToCurrentResultStateEvent>(
                _ => _stateMachine.ChangeState(_currentResultState));
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_setResultStateToken);
            BattleEventBus.UnsubscribeEvent(_changeResultStateToken);
        }

        private void SetBattleResult(EBattleResult result)
        {
            if (!_resultStatesDict.TryGetValue(result, out var resultState))
            {
                Debug.LogError($"Result state with {result} not found");                
                return;
            }
            _currentResultState = resultState;
            resultState.RaiseSetEvent();
        }
    }
}