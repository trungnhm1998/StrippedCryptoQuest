using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Item.MagicStone;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class UpgradeMagicStoneSystem : MonoBehaviour
    {
        [field: SerializeField] public MagicStoneInventory MagicStoneInventory { get; private set; }
        [field: SerializeField] public StoneUpgradePresenter StoneUpgradePresenter { get; private set; }
        [field: SerializeField] public UIUpgradableStoneList UpgradableStoneListUI { get; private set; }
        [field: SerializeField] public UIMaterialStoneList MaterialStoneListUI { get; private set; }
        [field: SerializeField] public StoneListPresenter ListPresenter { get; private set; }
        [field: SerializeField] public ConfirmStoneUpgradePresenter ConfirmUpgradePresenter { get; private set; }
        [field: SerializeField] public UIUpgradeMagicStoneToolTip MagicStoneTooltip { get; private set; }

        [field: SerializeField] public LocalizedString SelectStoneToUpdateText { get; private set; }
        [field: SerializeField] public LocalizedString SelectStoneText { get; private set; }
        [field: SerializeField] public LocalizedString ConfirmUpgradeText { get; private set; }

        public IMagicStone GetUpgradedStone(IMagicStone stoneToUpgrade)
        {
            //TODO: implement mapping logic here 
            return stoneToUpgrade;
        }

        public List<IMagicStone> GetUpgradableStones()
        {
            return MagicStoneInventory.MagicStones
                .GroupBy(item => item.ID)
                .Where(group => group.Count() >= 3)
                .SelectMany(group => group.ToList())
                .ToList();
        }

        public UpgradableStoneData GetUpgradableStoneData(IMagicStone stoneToUpgrade)
        {
            //TODO: implement mapping logic here 
            return new UpgradableStoneData()
            {
                ID = stoneToUpgrade.Level,
                Level = stoneToUpgrade.Level,
                Gold = 10,
                Metad = 100,
                Probability = 100
            };
        }
    }
}