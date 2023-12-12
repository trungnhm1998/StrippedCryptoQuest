using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.Menus.DimensionalBox.UI.EquipmentsTransfer;
using CryptoQuest.Sagas.Objects;
using FSM;
using IndiGames.Core.Events;
using TinyMessenger;

namespace CryptoQuest.Menus.DimensionalBox.States.EquipmentsTransfer
{
    public enum EEquipmentState
    {
        SelectEquipment = 0,
        Confirm,
        Overview
    }

    public class TransferEquipmentsStateMachine : StateMachine<EState, EEquipmentState, EStateAction>
    {
        private DBoxStateMachine _rootFsm;
        public InputMediatorSO Input => _rootFsm.Panel.Input;
        public TransferEquipmentsPanel Panel => _rootFsm.Panel.EquipmentsTransferPanel;
        public UIEquipmentList IngameList => Panel.IngameList;
        public UIEquipmentList InboxList => Panel.InboxList;
        public List<UIEquipment> ToWallet { get; set; }
        public List<UIEquipment> ToGame { get; set; }

        public TransferEquipmentsStateMachine(DBoxStateMachine rootFsm) : base(false)
        {
            _rootFsm = rootFsm;

            AddState(EEquipmentState.SelectEquipment, new SelectEquipment(this));
            AddState(EEquipmentState.Confirm, new Confirm(this));
            AddState(EEquipmentState.Overview, new State<EEquipmentState>(onEnter: BackToOverview));

            SetStartState(EEquipmentState.SelectEquipment);
        }

        private TinyMessageSubscriptionToken _fetchIngame;
        private TinyMessageSubscriptionToken _fetchInbox;
        private TinyMessageSubscriptionToken _transferringEvent;

        public override void OnEnter()
        {
            IngameList.Clear();
            InboxList.Clear();
            _hasFocus = false;
            HideOverviewPanel();
            Panel.gameObject.SetActive(true);

            _fetchIngame =
                ActionDispatcher.Bind<FetchIngameEquipmentsSuccess>(ctx => FillEquipments(IngameList, ctx.Equipments));
            _fetchInbox =
                ActionDispatcher.Bind<FetchInboxEquipmentsSuccess>(ctx => FillEquipments(InboxList, ctx.Equipments));
            _transferringEvent = ActionDispatcher.Bind<TransferringEquipments>(_ => _hasFocus = false);

            ActionDispatcher.Dispatch(new FetchNftEquipments());
            base.OnEnter();
        }

        private bool _hasFocus;

        private void FillEquipments(UIEquipmentList uiList, EquipmentResponse[] equipments)
        {
            uiList.Initialize(equipments);
            uiList.Interactable = false;
            if (_hasFocus)
            {
                _hasFocus = false;
                return;
            }
            _hasFocus = uiList.TryFocus();
        }

        public override void OnExit()
        {
            Panel.gameObject.SetActive(false);
            base.OnExit();

            ActionDispatcher.Unbind(_fetchIngame);
            ActionDispatcher.Unbind(_fetchInbox);
            ActionDispatcher.Unbind(_transferringEvent);
        }

        private void BackToOverview(State<EEquipmentState, string> _) => fsm.RequestStateChange(EState.Overview);
        private void HideOverviewPanel() => _rootFsm.Panel.OverviewPanel.SetActive(false);
    }
}