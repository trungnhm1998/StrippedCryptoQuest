using CryptoQuest.Input;
using CryptoQuest.Menus.Beast.States;
using FSM;
using Input;
using UnityEngine;

namespace CryptoQuest.Menus.Beast.UI
{
    public class UIBeastMenu : MonoBehaviour
    {
        [field: SerializeField, Header("State Context")] public InputMediatorSO Input { get; private set; }
        private StateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = new BeastMenuStateMachine(this);
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
