using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.UpgradeStoneUI.UI
{
    public class StoneUpgradeSystem : MonoBehaviour
    {
        [field: SerializeField] public StoneUpgradePresenter StoneUpgradePresenter { get; private set; }
        [field: SerializeField] public UIUpgradableStoneList UpgradableStoneListUI { get; private set; }

        [field: SerializeField] public LocalizedString SelectStoneToUpdateText { get; private set; }
        [field: SerializeField] public LocalizedString SelectStoneText { get; private set; }
        [field: SerializeField] public LocalizedString ConfirmEvolveText { get; private set; }
    }
}