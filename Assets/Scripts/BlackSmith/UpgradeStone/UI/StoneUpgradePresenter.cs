using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class StoneUpgradePresenter : MonoBehaviour
    {
        [SerializeField] private UIUpgradableStoneList _upgradableStoneListUI;

        [field: SerializeField] public BlackSmithDialogsPresenter DialogPresenter { get; private set; }

        [field: SerializeField, Header("Localization")]
        public LocalizedString SelectBaseMessage { get; private set; }

        [field: SerializeField] public LocalizedString SelectMaterialMessage { get; private set; }
    }
}