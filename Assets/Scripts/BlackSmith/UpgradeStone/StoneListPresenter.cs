using System.Collections.Generic;
using CryptoQuest.BlackSmith.UpgradeStone.UI;
using CryptoQuest.Item.MagicStone;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone
{
    public class StoneListPresenter : MonoBehaviour
    {
        [SerializeField] private UpgradableStonesPresenter _upgradableStonePresenter;
        [SerializeField] private UIUpgradeMagicStoneToolTip _magicStoneTooltip;

        private void OnEnable()
        {
            _upgradableStonePresenter.StoneSelected += OnSelectStone;
            _upgradableStonePresenter.StoneInspected += PreviewBaseData;
            _upgradableStonePresenter.StoneDeselected += OnDeselect;
        }

        private void OnSelectStone(UIUpgradableStone stoneUI) { }

        private void OnDisable()
        {
            _upgradableStonePresenter.StoneSelected -= OnSelectStone;
            _upgradableStonePresenter.StoneInspected -= PreviewBaseData;
            _upgradableStonePresenter.StoneDeselected -= OnDeselect;
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