using CryptoQuest.BlackSmith.Evolve.UI;
using CryptoQuest.Item.MagicStone;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class UpgradeStoneResultPresenter : MonoBehaviour
    {
        [field: SerializeField] public UIResultPanel ResultPanelUI { get; private set; }
        [field: SerializeField] public UIUpgradeMagicStoneToolTip StoneDetailPanelUI { get; private set; }
        public IMagicStone UpgradedStone { get; set; }

        private void OnDisable()
        {
            UpgradedStone = null;
            StoneDetailPanelUI.gameObject.SetActive(false);
        }

        public void SetResultFail()
        {
            ResultPanelUI.UpdateUIFail();
            StoneDetailPanelUI.gameObject.SetActive(false);
        }

        public void SetResultSuccess()
        {
            if (UpgradedStone == null) return;

            ResultPanelUI.UpdateUISuccess();
            StoneDetailPanelUI.SetData(UpgradedStone, false);
            StoneDetailPanelUI.gameObject.SetActive(true);
        }
    }
}