using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.Menus.DimensionalBox.UI.MagicStoneTransfer;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Tooltips.Events;
using FSM;
using IndiGames.Core.Events;
using TinyMessenger;

namespace CryptoQuest.Menus.DimensionalBox.States.MagicStoneTransfer
{
    public enum EMagicStoneState
    {
        SelectStone = 0,
        Confirm = 1,
        Overview = 2
    }

    public class TransferringMagicStoneStateMachine : StateMachine<EState, EMagicStoneState, EStateAction>
    {
        private DBoxStateMachine _rootFsm;
        private bool _hasFocus;
        public InputMediatorSO Input => _rootFsm.Panel.Input;
        public TransferMagicStonesPanel Panel => _rootFsm.Panel.TransferMagicStonesPanel;
        public ShowTooltipEvent ShowTooltipEventChannel => Panel.ShowTooltipEventChannel;
        public UIMagicStoneList IngameList => Panel.IngameList;
        public UIMagicStoneList DBoxList => Panel.InboxList;

        public List<UIMagicStone> ToWallet { get; set; }
        public List<UIMagicStone> ToGame { get; set; }

        private TinyMessageSubscriptionToken _fetchIngame;
        private TinyMessageSubscriptionToken _fetchInbox;
        private TinyMessageSubscriptionToken _transferringEvent;

        public TransferringMagicStoneStateMachine(DBoxStateMachine rootFsm) : base(false)
        {
            _rootFsm = rootFsm;

            AddState(EMagicStoneState.SelectStone, new SelectStone(this));
            AddState(EMagicStoneState.Confirm, new ConfirmMagicStoneTransfer(this));
            AddState(EMagicStoneState.Overview, new State<EMagicStoneState>(BackToOverview));

            SetStartState(EMagicStoneState.SelectStone);
        }

        public override void OnEnter()
        {
            IngameList.Clear();
            DBoxList.Clear();

            _hasFocus = false;

            HideOverviewPanel();
            Panel.gameObject.SetActive(true);

            _fetchIngame =
                ActionDispatcher.Bind<FetchIngameMagicStonesSuccess>(ctx => FillStones(IngameList, ctx.Stones));
            _fetchInbox =
                ActionDispatcher.Bind<FetchInboxMagicStonesSuccess>(ctx => FillStones(DBoxList, ctx.Stones));
            _transferringEvent = ActionDispatcher.Bind<TransferringMagicStones>(_ => _hasFocus = false);

            ActionDispatcher.Dispatch(new FetchProfileMagicStonesAction());
            base.OnEnter();
        }

        public override void OnExit()
        {
            Panel.gameObject.SetActive(false);
            base.OnExit();

            ActionDispatcher.Unbind(_fetchIngame);
            ActionDispatcher.Unbind(_fetchInbox);
            ActionDispatcher.Unbind(_transferringEvent);
        }

        private void FillStones(UIMagicStoneList uiList, MagicStone[] stones)
        {
            uiList.Initialize(stones);
            uiList.Interactable = false;
            if (_hasFocus)
            {
                _hasFocus = false;
                return;
            }

            _hasFocus = uiList.TryFocus();
        }

        private void BackToOverview(State<EMagicStoneState, string> _) => _rootFsm.RequestStateChange(EState.Overview);
        private void HideOverviewPanel() => _rootFsm.Panel.OverviewPanel.SetActive(false);
    }
}