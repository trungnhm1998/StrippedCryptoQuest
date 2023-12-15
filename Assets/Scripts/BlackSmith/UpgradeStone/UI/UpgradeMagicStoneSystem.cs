using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item.MagicStone;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class UpgradeMagicStoneSystem : MonoBehaviour
    {
        public List<IMagicStone> StoneList { get; set; } = new();
      
        [field: SerializeField] public CurrencyPresenter CurrencyPresenter { get; private set; }
        [field: SerializeField] public StoneUpgradePresenter StoneUpgradePresenter { get; private set; }
        [field: SerializeField] public UpgradableStonesPresenter UpgradableStonesPresenter { get; private set; }
        [field: SerializeField] public MaterialStonesPresenter MaterialStonesPresenter { get; private set; }
        [field: SerializeField] public StoneListPresenter ListPresenter { get; private set; }
        [field: SerializeField] public ConfirmStoneUpgradePresenter ConfirmUpgradePresenter { get; private set; }
        [field: SerializeField] public UIUpgradeMagicStoneToolTip MagicStoneTooltip { get; private set; }
        [field: SerializeField] public UpgradeStoneResultPresenter UpgradeStoneResultPresenter { get; private set; }

        [field: SerializeField] public LocalizedString SelectStoneToUpdateText { get; private set; }
        [field: SerializeField] public LocalizedString SelectMaterialText { get; private set; }
        [field: SerializeField] public LocalizedString ConfirmUpgradeText { get; private set; }
        [field: SerializeField] public LocalizedString UpgradeSuccessText { get; private set; }
        [field: SerializeField] public LocalizedString UpgradeFailedText { get; private set; }

        public IMagicStone GetUpgradedStone(IMagicStone stoneToUpgrade)
        {
            //TODO: implement mapping logic here (API preview) 
            return stoneToUpgrade;
        }

        public List<IMagicStone> GetUpgradableStones()
        {
            return StoneList
                .GroupBy(item => new { item.Definition, item.Level })
                .Where(group => group.Count() >= 3)
                .SelectMany(group => group.ToList())
                .ToList();
        }
    }
}