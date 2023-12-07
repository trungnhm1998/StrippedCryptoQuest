using System;
using CryptoQuest.Menus.DimensionalBox.States.EquipmentsTransfer;
using CryptoQuest.Menus.DimensionalBox.States.MagicStoneTransfer;
using FSM;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.States
{
    public enum EState
    {
        Overview = 0,
        Equipment = 1,
        Metad = 2,
        MagicStone = 3,
    }

    public enum EStateAction
    {
        OnCancel = 0,
        OnExecute = 1,
        OnNavigate = 2,
        OnReset = 3,
    }

    public class DBoxStateMachine : StateMachine<EState, EStateAction>
    {
        public MenuPanel Panel { get; }

        public DBoxStateMachine(MenuPanel menuPanel)
        {
            Panel = menuPanel;
            AddState(EState.Overview, new Overview(menuPanel));
            AddState(EState.Equipment, new TransferEquipmentsStateMachine(this));
            AddState(EState.Metad, new Metad(menuPanel));
            AddState(EState.MagicStone, new TransferringMagicStoneStateMachine(this));

            SetStartState(EState.Overview);
        }

        public void Navigate(Vector2 axis) => OnAction(EStateAction.OnNavigate, axis);

        public void Execute() => OnAction(EStateAction.OnExecute);

        public void Cancel()
        {
            try
            {
                OnAction(EStateAction.OnCancel);
            }
            catch (Exception e)
            {
                // UnityAction cache the subscribers list
            }
        }

        public void Reset() => OnAction(EStateAction.OnReset);
    }

    public class Metad : DBoxStateBase
    {
        public Metad(MenuPanel menuPanel) : base(menuPanel) { }
    }
}