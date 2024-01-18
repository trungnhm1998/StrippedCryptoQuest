using System;
using UnityEngine;

namespace CryptoQuest.Battle.States
{
    public interface IResultHandler
    {
        ResultSO.EState ResultState { get; }
        bool TryEndBattle();
    }

    [Serializable]
    public class DoNothingThenEnd : IResultHandler
    {
        public virtual ResultSO.EState ResultState { get; }
        public virtual bool TryEndBattle() => true;
    }

    [Serializable]
    public class HandleLose : DoNothingThenEnd
    {
        public override ResultSO.EState ResultState => ResultSO.EState.Lose;
    }

    [Serializable]
    public class HandleRetreat : DoNothingThenEnd
    {
        public override ResultSO.EState ResultState => ResultSO.EState.Retreat;
    }

    [Serializable]
    public class HandleNoResult : IResultHandler
    {
        [SerializeField] private BattleStateMachine _stateMachine;

        public ResultSO.EState ResultState => ResultSO.EState.None;
        public virtual bool TryEndBattle()
        {
            _stateMachine.ChangeState(new SelectHeroesActions.SelectHeroesActions());
            return false;
        }
    }

    [Serializable]
    public class HandleWin : IResultHandler
    {
        [SerializeField] private BattleLootManager _lootManager;
        [SerializeField] private ResultSO _result;

        public ResultSO.EState ResultState => ResultSO.EState.Win;
        public virtual bool TryEndBattle()
        {
            _result.Loots = _lootManager.GetDroppedLoots();
            return true;
        }
    }

    [Serializable]
    public class HandleLostInQuest : DoNothingThenEnd
    {
        public override ResultSO.EState ResultState => ResultSO.EState.LoseInQuest;
    }
}