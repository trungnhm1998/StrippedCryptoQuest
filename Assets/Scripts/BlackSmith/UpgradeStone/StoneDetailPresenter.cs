using System.Collections.Generic;
using CryptoQuest.BlackSmith.UpgradeStone.UI;
using CryptoQuest.Item.MagicStone;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone
{
    public class StoneDetailPresenter : MonoBehaviour
    {
        [SerializeField] private UIUpgradableStoneList _upgradableStoneListUI;
        [SerializeField] private UIUpgradeMagicStoneToolTip _magicStoneTooltip;
        [SerializeField] private UIMaterialStoneList _materialStoneList;
        [SerializeField] private UpgradeMagicStoneSystem _upgradeMagicStoneSystem;
        [SerializeField] private int _requiredMaterialAmount = 3;
        private List<UIUpgradableStone> _selectedMaterials = new();
        private UIUpgradableStone _selectedStone;

        private void OnEnable()
        {
            _upgradableStoneListUI.StoneSelected += OnSelectStone;
            _upgradableStoneListUI.StoneInspected += PreviewBaseData;
            _upgradableStoneListUI.StoneDeselected += OnDeselect;
            _materialStoneList.MaterialSelected += OnSelectMaterial;
            _materialStoneList.ResetMaterial += OnResetMaterial;
        }

        private void OnSelectStone(UIUpgradableStone stoneUI)
        {
            _selectedStone = stoneUI;
        }

        private void OnDisable()
        {
            _upgradableStoneListUI.StoneSelected -= OnSelectStone;
            _upgradableStoneListUI.StoneInspected -= PreviewBaseData;
            _upgradableStoneListUI.StoneDeselected -= OnDeselect;
            _materialStoneList.MaterialSelected -= OnSelectMaterial;
            _materialStoneList.ResetMaterial += OnResetMaterial;
        }

        private void OnResetMaterial()
        {
            _selectedMaterials.Clear();
            _selectedStone = null;
        }

        private void OnDeselect()
        {
            SetActiveTooltip(false);
        }

        private void OnSelectMaterial(UIUpgradableStone stoneUI)
        {
            if (stoneUI == null) return;
            if (_selectedMaterials.Contains(stoneUI))
            {
                DeselectMaterial(stoneUI);
                return;
            }

            if (_selectedMaterials.Count >= _requiredMaterialAmount) return;
            SelectMaterial(stoneUI);
        }

        private void SelectMaterial(UIUpgradableStone stoneUI)
        {
            stoneUI.Highlight(true);
            _selectedMaterials.Add(stoneUI);

            if (_selectedMaterials.Count == _requiredMaterialAmount)
            {
                var upgradedStone = GetUpgradedStone(_selectedStone.MagicStone);
                PreviewUpdatedStone(upgradedStone);
            }
        }

        private void DeselectMaterial(UIUpgradableStone stoneUI)
        {
            _selectedMaterials.Remove(stoneUI);
            stoneUI.Highlight(false);
            PreviewBaseData(_selectedStone.MagicStone);
        }

        private void PreviewBaseData(IMagicStone stone)
        {
            _magicStoneTooltip.SetData(stone, true);
            SetActiveTooltip(false);
            SetActiveTooltip(true);
        }

        private void PreviewUpdatedStone(IMagicStone stone)
        {
            _magicStoneTooltip.SetData(stone, false);
            SetActiveTooltip(false);
            SetActiveTooltip(true);
        }

        private IMagicStone GetUpgradedStone(IMagicStone stone)
        {
            return _upgradeMagicStoneSystem.GetUpgradedStone(stone);
        }

        private void SetActiveTooltip(bool value)
        {
            _magicStoneTooltip.gameObject.SetActive(value);
        }
    }
}