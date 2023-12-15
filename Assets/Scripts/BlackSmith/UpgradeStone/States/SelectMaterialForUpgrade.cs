using System.Collections.Generic;
using CryptoQuest.BlackSmith.UpgradeStone.UI;
using CryptoQuest.Item.MagicStone;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public class SelectMaterialForUpgrade : UpgradeMagicStoneStateBase
    {
        public SelectMaterialForUpgrade(UpgradeMagicStoneStateMachine stateMachine) : base(stateMachine) { }
        private List<UIUpgradableStone> _cachedItems = new();
        private readonly int _requiredMaterialAmount = 3;

        public override void OnEnter()
        {
            base.OnEnter();
            ResetMaterials();
            var upgradableStones = _stateMachine.UpgradeMagicStoneSystem.GetUpgradableStones();
            _dialogsPresenter.Dialogue.SetMessage(_stateMachine.UpgradeMagicStoneSystem.SelectMaterialText).Show();

            _magicStoneTooltip.SetData(_stateMachine.StoneToUpgrade.MagicStone, true);
            _magicStoneTooltip.SetupInfo();
            _materialStonesPresenter.gameObject.SetActive(true);

            _materialStonesPresenter.ClearStones();
            _materialStonesPresenter.RenderStones(upgradableStones);
            _materialStonesPresenter.ClearStones(_stateMachine.StoneToUpgrade);
            _materialStonesPresenter.MaterialSelected += OnSelectMaterialStone;
        }

        private void OnSelectMaterialStone(UIUpgradableStone stoneUI)
        {
            if (stoneUI == null) return;
            if (_cachedItems.Contains(stoneUI))
            {
                DeselectMaterial(stoneUI);
                return;
            }

            if (_cachedItems.Count >= _requiredMaterialAmount) return;
            SelectMaterial(stoneUI);
        }

        private void SelectMaterial(UIUpgradableStone stoneUI)
        {
            _cachedItems.Add(stoneUI);
            stoneUI.Highlight(true);
            if (_cachedItems.Count != _requiredMaterialAmount) return;
            _stateMachine.SelectedMaterials = new List<UIUpgradableStone>(_cachedItems);
            _listPresenter.PreviewUpdatedStone(GetUpgradedStone(_stateMachine.StoneToUpgrade.MagicStone));
            fsm.RequestStateChange(EUpgradeMagicStoneStates.ConfirmUpgrade);
        }

        private void DeselectMaterial(UIUpgradableStone stoneUI)
        {
            _cachedItems.Remove(stoneUI);
            stoneUI.Highlight(false);
            _listPresenter.PreviewBaseData(_stateMachine.StoneToUpgrade.MagicStone);
        }

        public override void OnExit()
        {
            base.OnExit();
            _cachedItems.Clear();
            _materialStonesPresenter.MaterialSelected -= OnSelectMaterialStone;
        }

        private IMagicStone GetUpgradedStone(IMagicStone stone)
        {
            return _stateMachine.UpgradeMagicStoneSystem.GetUpgradedStone(stone);
        }


        public override void OnCancel()
        {
            _stateMachine.StoneToUpgrade = null;
            _materialStonesPresenter.ClearStones();
            _materialStonesPresenter.MaterialSelected -= OnSelectMaterialStone;
            fsm.RequestStateChange(EUpgradeMagicStoneStates.SelectStone);
            ResetMaterials();
        }

        private void ResetMaterials()
        {
            _stateMachine.SelectedMaterials.Clear();
            _cachedItems.Clear();
        }
    }
}