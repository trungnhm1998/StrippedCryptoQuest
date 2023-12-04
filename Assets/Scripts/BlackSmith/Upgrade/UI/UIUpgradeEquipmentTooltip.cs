using UnityEngine;
using CryptoQuest.UI.Tooltips.Equipment;
using CryptoQuest.BlackSmith.Upgrade.Presenters;

namespace CryptoQuest.BlackSmith.Upgrade.UI
{
    public class UIUpgradeEquipmentTooltip : UIEquipmentTooltip
    {
        [SerializeField] private EquipmentDetailsPresenter _equipmentProvider;

        public string LevelText => _levelTextFormat;

        protected override bool CanShow()
        {
            if (_equipmentProvider == null) return false;
            if (_equipmentProvider.Equipment == null || !_equipmentProvider.Equipment.IsValid())
                return false;
            _equipment = _equipmentProvider.Equipment;
            return true;
        }
    }
}