using System.Collections.Generic;
using CryptoQuest.BlackSmith.UpgradeStone.UI;
using FSM;

namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public enum EUpgradeMagicStoneStates
    {
        SelectStone = 0,
        SelectMaterialStone = 1,
        ConfirmUpgrade = 2
    }

    public class UpgradeMagicStoneStateMachine : StateMachine<string, EUpgradeMagicStoneStates, string>
    {
        public UIUpgradableStoneList UpgradableStoneListUI => UpgradeMagicStoneSystem.UpgradableStoneListUI;
        public UIMaterialStoneList MaterialStoneListUI => UpgradeMagicStoneSystem.MaterialStoneListUI;
        public BlackSmithDialogsPresenter DialogsPresenter => _context.DialogPresenter;
        public UIUpgradeMagicStoneToolTip MagicStoneTooltip => UpgradeMagicStoneSystem.MagicStoneTooltip;

        public ConfirmStoneUpgradePresenter ConfirmUpgradePresenter =>
            UpgradeMagicStoneSystem.ConfirmUpgradePresenter;

        public StoneListPresenter ListPresenter => UpgradeMagicStoneSystem.ListPresenter;

        private readonly BlackSmithSystem _context;
        public UpgradeMagicStoneSystem UpgradeMagicStoneSystem { get; }
        private UpgradeMagicStoneStateBase _state;
        public UIUpgradableStone StoneToUpgrade { get; set; }
        public List<UIUpgradableStone> SelectedMaterials { get; set; } = new();

        public UpgradeMagicStoneStateMachine(BlackSmithSystem context)
        {
            _context = context;
            UpgradeMagicStoneSystem = context.UpgradeMagicStoneSystem;
            AddState(EUpgradeMagicStoneStates.SelectStone, new SelectStoneToUpgrade(this));
            AddState(EUpgradeMagicStoneStates.SelectMaterialStone, new SelectMaterialForUpgrade(this));
            AddState(EUpgradeMagicStoneStates.ConfirmUpgrade, new ConfirmUpgradeStone(this));
            SetStartState(EUpgradeMagicStoneStates.SelectStone);
        }

        public override void OnEnter()
        {
            UpgradeMagicStoneSystem.gameObject.SetActive(true);
            _context.Input.CancelEvent += OnCancel;
            base.OnEnter();
        }

        public void SetCurrentState(UpgradeMagicStoneStateBase state) => _state = state;

        public override void OnExit()
        {
            UpgradeMagicStoneSystem.gameObject.SetActive(false);
            _context.Input.CancelEvent -= OnCancel;
            base.OnExit();
        }

        private void OnCancel() => _state?.OnCancel();

        public void BackToOverview() => fsm.RequestStateChange(State.OVERVIEW);
    }
}