using System.Collections.Generic;
using CryptoQuest.BlackSmith.UpgradeStone.UI;
using CryptoQuest.Item.MagicStone;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone
{
    public class StoneListPresenter : MonoBehaviour
    {
        [SerializeField] private UIUpgradableStoneList _upgradableStoneListUI;
        [SerializeField] private UIUpgradeMagicStoneToolTip _magicStoneTooltip;

        private void OnEnable()
        {
            _upgradableStoneListUI.StoneSelected += OnSelectStone;
            _upgradableStoneListUI.StoneInspected += PreviewBaseData;
            _upgradableStoneListUI.StoneDeselected += OnDeselect;
        }

        private void OnSelectStone(UIUpgradableStone stoneUI) { }

        private void OnDisable()
        {
            _upgradableStoneListUI.StoneSelected -= OnSelectStone;
            _upgradableStoneListUI.StoneInspected -= PreviewBaseData;
            _upgradableStoneListUI.StoneDeselected -= OnDeselect;
        }

        private void OnDeselect()
        {
            SetActiveTooltip(false);
        }


        public void PreviewBaseData(IMagicStone stone)
        {
            _magicStoneTooltip.SetData(stone, true);
            SetActiveTooltip(false);
            SetActiveTooltip(true);
        }

        public void PreviewUpdatedStone(IMagicStone stone)
        {
            _magicStoneTooltip.SetData(stone, false);
            SetActiveTooltip(false);
            SetActiveTooltip(true);
        }

        private void SetActiveTooltip(bool value)
        {
            _magicStoneTooltip.gameObject.SetActive(value);
        }
    }
}