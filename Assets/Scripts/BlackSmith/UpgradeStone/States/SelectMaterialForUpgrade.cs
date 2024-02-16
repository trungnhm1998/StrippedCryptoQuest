using System.Collections.Generic;
using CryptoQuest.BlackSmith.UpgradeStone.Sagas;
using CryptoQuest.BlackSmith.UpgradeStone.UI;
using CryptoQuest.Item.MagicStone;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public class SelectMaterialForUpgrade : UpgradeMagicStoneStateBase
    {
        public SelectMaterialForUpgrade(UpgradeMagicStoneStateMachine stateMachine) : base(stateMachine)
        {
        }

        private List<UIUpgradableStone> _cachedItems = new();
        private readonly int _requiredMaterialAmount = 3;

        public override void OnEnter()
        {
            base.OnEnter();
            ResetMaterials();
            var upgradableStones = _stateMachine.UpgradeMagicStoneSystem.GetUpgradableStones();
            _dialogsPresenter.Dialogue.SetMessage(_stateMachine.UpgradeMagicStoneSystem.SelectMaterialText).Show();
            RequestPreview(upgradableStones, _stateMachine.StoneToUpgrade.MagicStone);
            _magicStoneTooltip.SetData(_stateMachine.StoneToUpgrade.MagicStone, true);
            _magicStoneTooltip.SetupInfo();
            _magicStoneTooltip.gameObject.SetActive(true);
            _materialStonesPresenter.gameObject.SetActive(true);
            _upgradableStonePresenter.ClearStones();
            _materialStonesPresenter.ClearStones();
            _materialStonesPresenter.RenderStones(upgradableStones);
            _materialStonesPresenter.ClearStones(_stateMachine.StoneToUpgrade);
            _materialStonesPresenter.MaterialSelected += OnSelectMaterialStone;
            _materialStonesPresenter.SelectFirstButton();
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
            return _stateMachine.UpgradeMagicStoneSystem.UpgradeStoneModel.GetUpgradedStone(stone);
        }


        public override void OnCancel()
        {
            _stateMachine.StoneToUpgrade = null;
            _materialStonesPresenter.ClearStones();
            _materialStonesPresenter.MaterialSelected -= OnSelectMaterialStone;
            fsm.RequestStateChange(EUpgradeMagicStoneStates.SelectStone);
            ResetMaterials();
        }

        private void RequestPreview(List<IMagicStone> stoneLists, IMagicStone stoneToUpgrade)
        {
            if (_stateMachine.UpgradeMagicStoneSystem.UpgradeStoneModel.TryGetPreview(stoneToUpgrade,
                    out var preview))
                return;


            List<IMagicStone> filteredStoneLists = new();
            foreach (var stone in stoneLists)
            {
                if (!stone.IsEqual(stoneToUpgrade)) continue;
                filteredStoneLists.Add(stone);
            }


            var ids = GetIdsForPreviews(filteredStoneLists);
            if (ids.Count == 0) return;
            ActionDispatcher.Dispatch(new UpgradeStonePreviewRequest(ids));
        }

        private List<int> GetIdsForPreviews(List<IMagicStone> stoneList)
        {
            if (stoneList.Count < 3) return new List<int>();
            List<int> ids = new();
            for (int i = 0; i < 3; i++)
            {
                ids.Add(stoneList[i].ID);
            }

            return ids;
        }

        private void ResetMaterials()
        {
            _stateMachine.SelectedMaterials.Clear();
            _cachedItems.Clear();
        }
    }
}