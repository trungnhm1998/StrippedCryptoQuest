using CryptoQuest.BlackSmith.Upgrade.Presenters;
using CryptoQuest.BlackSmith.Upgrade.UI;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UpgradeSystem : MonoBehaviour
    {
        [field: SerializeField] public EquipmentsPresenter EquipmentsPresenter { get; private set; }
        [field: SerializeField] public EquipmentDetailsPresenter EquipmentDetailsPresenter { get; private set; }
        [field: SerializeField] public ConfigUpgradePresenter ConfigUpgradePresenter { get; private set; }
        [field: SerializeField] public UIConfirmDetails UIConfirmDetails { get; private set; }
        [field: SerializeField] public GameObject ResultUI { get; private set; }
        [field: SerializeField] public CurrencyPresenter CurrencyPresenter { get; private set; }

        [field: SerializeField, Header("Localization")]
        public LocalizedString SelectEquipmentToUpgradeText { get; private set; }
        [field: SerializeField] public LocalizedString ConfigUpgradeText { get; private set; }
        [field: SerializeField] public LocalizedString ConfirmUpgradeText { get; private set; }
        [field: SerializeField] public LocalizedString UpgradeResultText { get; private set; }
    }
}