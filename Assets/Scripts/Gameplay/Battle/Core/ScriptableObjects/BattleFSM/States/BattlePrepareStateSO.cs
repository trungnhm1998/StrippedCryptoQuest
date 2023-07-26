using CryptoQuest.FSM;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;
using System.Collections;
using CryptoQuest.Events;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.BattleFSM.States
{
    [CreateAssetMenu(fileName = "BattlePrepareStateSO", menuName = "Gameplay/Battle/FSM/States/Battle Prepare State")]
    public class BattlePrepareStateSO : BattleStateSO
    {
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private float _waitShowTime = 3f;
        [SerializeField] private LocalizedString _monsterAppear;
        [Header("Raise Events")]
        [SerializeField] private VoidEventChannelSO _showNextMarkEventChannel;
        [SerializeField] private LocalizedStringEventChannelSO _showBattleDialogEventChannel;
        [SerializeField] private VoidEventChannelSO _closeBattleDialogEventChannel;

        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _doneShowDialogEventChannel;

        private Coroutine _unitPrepareCoroutine; 
        private BaseStateMachine _stateMachine;

        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            _doneShowDialogEventChannel.EventRaised += OnDoneShowDialog;
            _stateMachine = stateMachine;
            _unitPrepareCoroutine = stateMachine.StartCoroutine(BattlePrepare(stateMachine));
        }

        private IEnumerator BattlePrepare(BaseStateMachine stateMachine)
        {
            if (BattleManager.BattleSpawner)
            {
                BattleManager.BattleSpawner.SpawnBattle();
            }
            BattleManager.InitBattleTeams();
            _gameState.UpdateGameState(EGameState.Battle);

            _showBattleDialogEventChannel.RaiseEvent(_monsterAppear);

            yield return new WaitForSeconds(_waitShowTime);

            _showNextMarkEventChannel.RaiseEvent();
        }

        private void OnDoneShowDialog()
        {
            _closeBattleDialogEventChannel.RaiseEvent();
            //Prevent press dialog confirm also press command button
            _stateMachine.StartCoroutine(DelayNextState(0.01f));
        }

        private IEnumerator DelayNextState(float delay = 0.01f)
        {
            yield return new WaitForSeconds(delay);
            _stateMachine.SetCurrentState(_nextState);
        }

        public override void OnExitState(BaseStateMachine stateMachine)
        {
            base.OnExitState(stateMachine);
            _doneShowDialogEventChannel.EventRaised -= OnDoneShowDialog;
            stateMachine.StopCoroutine(_unitPrepareCoroutine);
        }
    }
}