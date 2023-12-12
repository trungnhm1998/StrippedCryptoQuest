using CryptoQuest.BlackSmith.UpgradeStone.UI;
using CryptoQuest.Item.MagicStone;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone
{
    public class StoneDetailPresenter : MonoBehaviour
    {
        [SerializeField] private UIUpgradableStoneList _upgradableStoneListUI;
        [SerializeField] private UIUpgradeMagicStoneToolTip _magicStoneTooltip;

        private void OnEnable()
        {
            _upgradableStoneListUI.StoneSelected += PreviewUpdatedStone;
            _upgradableStoneListUI.StoneDeselected += OnDeselect;
        }

        private void OnDisable()
        {
            _upgradableStoneListUI.StoneSelected -= PreviewUpdatedStone;
            _upgradableStoneListUI.StoneDeselected -= OnDeselect;
        }

        private void OnDeselect()
        {
            SetActiveTooltip(false);
        }

        private void PreviewBaseData(IMagicStone stone)
        {
            _magicStoneTooltip.SetData(stone, true);
            SetActiveTooltip(false);
            SetActiveTooltip(true);
        }

        public void PreviewUpdatedStone(IMagicStone stone)
        {
            _magicStoneTooltip.SetData(stone, false);
            SetActiveTooltip(true);
        }

        private void SetActiveTooltip(bool value)
        {
            _magicStoneTooltip.gameObject.SetActive(value);
        }
    }
}