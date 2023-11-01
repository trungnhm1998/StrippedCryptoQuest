using CryptoQuest.Input;
using CryptoQuest.Menus.Home.States;
using FSM;
using UnityEngine;

namespace CryptoQuest.Menus.Home.UI
{
    public class UIHomeMenu : MonoBehaviour
    {
        [Header("State Context")]
        [SerializeField] private UIHomeMenuSortCharacter _sortMode;
        [field: SerializeField] public InputMediatorSO Input { get; private set; }

        public UIHomeMenuSortCharacter SortMode => _sortMode;

        private StateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = new HomeMenuStateMachine(this);
        }

        private void OnEnable()
        {
            _stateMachine.Init();
        }

        private void OnDisable()
        {
            _stateMachine.OnExit();
        }
    }
}