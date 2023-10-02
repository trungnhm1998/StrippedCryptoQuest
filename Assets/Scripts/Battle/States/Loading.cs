using System.Collections;
using CryptoQuest.UI.Dialogs.BattleDialog;
using UnityEngine;

namespace CryptoQuest.Battle.States
{
    public class Loading : IState
    {
        private IBattleInitializer _initializer;
        private Coroutine _initCoroutine;
        private BattleStateMachine _battleStateMachine;

        public void OnEnter(BattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
            _initializer = battleStateMachine.GetComponent<IBattleInitializer>();
            GenericDialogController.Instance.CreateDialog(PromptCreated);
            InitBattle();
        }

        private Intro _introState;

        private void PromptCreated(UIGenericDialog dialog)
        {
            _introState = new Intro(dialog);
        }

        public void OnExit(BattleStateMachine battleStateMachine) { }

        private void InitBattle()
        {
            Debug.Log("BattleManager::InitBattle()");
            _initCoroutine = _battleStateMachine.StartCoroutine(CoInitBattle());
        }

        private IEnumerator CoInitBattle()
        {
            yield return _initializer.LoadEnemies();
            _battleStateMachine.Spiral.HideSpiral();
            yield return new WaitForSeconds(_battleStateMachine.Spiral.Duration);
            FinishInitBattle();
        }

        private void WinBattle()
        {
            // _battleStateMachine.BattleInput.DisableAllInput();
            // if (_initCoroutine != null) _battleStateMachine.StopCoroutine(_initCoroutine);
            // var context = new BattleContext();
            // List<LootInfo> loots = new();
            // var enemies = _initializer.Enemies;
            // // TODO: This also return cloned loot, but we already clone it in RewardManager?
            // foreach (var enemy in enemies)
            //     loots.AddRange(enemy.GetLoots());
            // if (loots.Count > 0) context.Loots = RewardManager.CloneAndMergeLoots(loots.ToArray());
            //
            // _battleStateMachine.BattleEndedEvent.RaiseEvent(context);
        }

        private void FinishInitBattle()
        {
            _battleStateMachine.BattleInput.EnableBattleInput();
            Debug.Log("BattleManager::Battle Initialized");

            _battleStateMachine.ChangeState(_introState); // TODO: Make sure dialog is loaded before change state
        }
    }
}