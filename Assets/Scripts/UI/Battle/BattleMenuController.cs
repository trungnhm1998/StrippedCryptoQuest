using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.System;
using UnityEngine;
using CryptoQuest.PushdownFSM;
using CryptoQuest.UI.Battle.StateMachine;
using FSM;
using CryptoQuest.UI.Battle.StartBattle;

namespace CryptoQuest.UI.Battle
{
    public class BattleMenuController : MonoBehaviour
    {
        [SerializeField] private ServiceProvider _serviceProvider;
        [SerializeField] private BattleInputSO _battleInput;
        [SerializeField] private BattleBus _battleBus;

        public InventorySO InventorySO => _serviceProvider.Inventory;

        public PushdownStateMachine BattleMenuFSM  { get; private set; }

        private void Start()
        {
            SetupStateMachine();
        }
        
        private void OnEnable()
        {
            _battleInput.CancelEvent += OnInputMenuCancel;
        }

        private void OnDisable()
        {
            _battleInput.CancelEvent -= OnInputMenuCancel;
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