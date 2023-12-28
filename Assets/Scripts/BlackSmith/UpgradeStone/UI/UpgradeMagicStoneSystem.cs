using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Item.MagicStone;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class UpgradeMagicStoneSystem : MonoBehaviour
    {
        public List<IMagicStone> StoneList => StoneInventory.MagicStones;
      
        [field: SerializeField] public MagicStoneInventory StoneInventory { get; private set; }
        [field: SerializeField] public CurrencyPresenter CurrencyPresenter { get; private set; }
        [field: SerializeField] public StoneUpgradePresenter StoneUpgradePresenter { get; private set; }
        [field: SerializeField] public UpgradableStonesPresenter UpgradableStonesPresenter { get; private set; }
        [field: SerializeField] public MaterialStonesPresenter MaterialStonesPresenter { get; private set; }
        [field: SerializeField] public StoneListPresenter ListPresenter { get; private set; }
        [field: SerializeField] public ConfirmStoneUpgradePresenter ConfirmUpgradePresenter { get; private set; }
        [field: SerializeField] public UIUpgradeMagicStoneToolTip MagicStoneTooltip { get; private set; }
        [field: SerializeField] public UpgradeStoneResultPresenter UpgradeStoneResultPresenter { get; private set; }
        [field: SerializeField] public UpgradeStoneModel UpgradeStoneModel { get; private set; }

        [field: SerializeField] public LocalizedString SelectStoneToUpdateText { get; private set; }
        [field: SerializeField] public LocalizedString SelectMaterialText { get; private set; }
        [field: SerializeField] public LocalizedString ConfirmUpgradeText { get; private set; }
        [field: SerializeField] public LocalizedString UpgradeSuccessText { get; private set; }
        [field: SerializeField] public LocalizedString UpgradeFailedText { get; private set; }

        public List<IMagicStone> GetUpgradableStones()
        {
            return StoneList
                .Where(item => item.AttachEquipmentId == 0)
                .GroupBy(item => new
                {
                    item.Definition,
                    item.Level,
                    PassiveId1 = item.Passives[0].Context.SkillInfo.Id,
                    PassiveId2 = item.Passives[1].Context.SkillInfo.Id
                })
                .Where(group => group.Count() >= 3)
                .SelectMany(group => group.ToList())
                .ToList();
        }
    }
}