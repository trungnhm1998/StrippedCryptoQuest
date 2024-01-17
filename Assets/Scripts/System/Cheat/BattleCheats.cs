using CommandTerminal;
using CryptoQuest.Battle;
using CryptoQuest.Battle.Events;
using UnityEngine;

namespace CryptoQuest.System.Cheat
{
    public class BattleCheats : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private ResultSO _result;
        
        public void InitCheats()
        {
            Debug.Log($"BattleCheats.InitCheats()");
            // Terminal.Shell.AddCommand("bat.killall", TriggerKillAll, 0, 0, "Kill all enemies");
            // Terminal.Shell.AddCommand("bat.kill", TriggerKillCharacter, 1, 1, "Kill character");
            Terminal.Shell.AddCommand("bat.win", TriggerWinBattle, 0, 0, "Instantly win current battle");
            Terminal.Shell.AddCommand("bat.lose", TriggerLoseBattle, 0, 0, "Instantly lose current battle");
            Terminal.Shell.AddCommand("bat.retreat", InstantlyRetreat, 0, 0, "Instantly retreat current battle");
        }

        private BattleStateMachine _battleStateMachine;
        private bool _isBattleMachineValid => _battleStateMachine != null && _battleStateMachine.gameObject != null;
        private BattleStateMachine _stateMachine
        {
            get
            {
                if (_isBattleMachineValid) return _battleStateMachine;
                _battleStateMachine = FindObjectOfType<BattleStateMachine>();
                return _battleStateMachine;
            }
        }

        private void TriggerLoseBattle(CommandArg[] obj)
        {
            _result.State = ResultSO.EState.Lose;
            _stateMachine.ChangeState(_stateMachine.ResultChecker);
        }

        private void TriggerWinBattle(CommandArg[] obj)
        {
            _result.State = ResultSO.EState.Win;
            _stateMachine.ChangeState(_stateMachine.ResultChecker);
        }

        private void InstantlyRetreat(CommandArg[] obj)
        {
            _result.State = ResultSO.EState.Retreat;
            _stateMachine.ChangeState(_stateMachine.ResultChecker);
        }
    }
}