using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.PushdownFSM;
using CryptoQuest.System;
using CryptoQuest.UI.Battle.StateMachine;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public class BattleMenuController : MonoBehaviour
    {
        [SerializeField] private BattleInputSO _battleInput;
        [SerializeField] private BattleBus _battleBus;

        public PushdownStateMachine BattleMenuFSM  { get; private set; }

        private void Start()
        {
            SetupStateMachine();
        }
        
        private void OnEnable()
        {
            _battleInput.EnableBattleInput();
            _battleInput.CancelEvent += OnInputMenuCancel;
        }

        private void OnDisable()
        {
            _battleInput.CancelEvent -= OnInputMenuCancel;
            _battleInput.DisableBattleInput();
        }

        public void ChangeState(string state)
        {
            BattleMenuFSM?.RequestStateChange(state);
        }

        private void SetupStateMachine()
        {
            BattleMenuFSM ??= new BattleMenuStateMachine(this);
        }
        
        private void OnInputMenuCancel()
        {
            BattleMenuFSM?.PushdownState();
        }
    }
}