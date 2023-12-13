using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.Menus.Beast.States;
using CryptoQuest.UI.Menu;
using FSM;
using UnityEngine;

namespace CryptoQuest.Menus.Beast.UI
{
    public class UIBeastMenu : UIMenuPanelBase
    {
        [field: SerializeField, Header("State Context")]
        public InputMediatorSO Input { get; private set; }

        [field: SerializeField] public BeastProvider BeastProvider { get; private set; }
        [field: SerializeField] public UIBeastList ListBeastUI { get; private set; }
        [field: SerializeField] public UIBeastDetail DetailBeastUI { get; private set; }

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