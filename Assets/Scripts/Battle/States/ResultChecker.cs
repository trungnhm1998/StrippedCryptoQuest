using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Battle.States
{
    public class ResultChecker : MonoBehaviour, IState
    {
        [SerializeField] private BattleLootManager _lootManager;
        [SerializeField] private BattleUnloader _unloader;
        [SerializeField] private ResultSO _result;
        [SerializeReference, SubclassSelector] private IResultHandler[] _resultHandlers;

        private Dictionary<ResultSO.EState, IResultHandler> _handlersDict = new();

        private void Awake()
        {
            foreach (var handler in _resultHandlers)
            {
                _handlersDict.TryAdd(handler.ResultState, handler);
            }
        }

        public void OnEnter(BattleStateMachine stateMachine)
        {
            var handler = _handlersDict[_result.State];
            if (!handler.TryEndBattle()) return;
            StartCoroutine(CoUnload());
        }

        public void OnExit(BattleStateMachine stateMachine) { }

        private IEnumerator CoUnload()
        {
            yield return _unloader.FadeInAndUnloadBattle();
        }
    }
}