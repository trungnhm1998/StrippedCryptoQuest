using CryptoQuest.UI.Dialogs.RewardDialog.ConfigDisplayAction;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization.Tables;

namespace CryptoQuest.UI.Dialogs.RewardDialog
{
    public class ConfigureMagicStoneLootDisplay : SagaBase<ConfigureMagicStoneLootDisplayAction>
    {
        [SerializeField] private MagicStoneDatabase _magicStoneDatabase;
        [SerializeField] private TableReference _tableReference;
        [SerializeField] private LocalizedString _itemWithQuantity;

        protected override void HandleAction(ConfigureMagicStoneLootDisplayAction ctx)
        {
            var msg = new LocalizedString(_itemWithQuantity.TableReference, _itemWithQuantity.TableEntryReference)
            {
                {
                    "item", GetMagicStoneString(ctx.Loot.StoneId)
                },
                {
                    "quantity", new IntVariable { Value = ctx.Loot.Quantity }
                }
            };

            ctx.UIRewardItem.SetContentStringRef(msg);
        }

        private LocalizedString GetMagicStoneString(string id)
        {
            var key = "";
            foreach (var magicStone in _magicStoneDatabase.sheets[0].list)
            {
                if (magicStone.stone_id != id) continue;
                key = magicStone.name_key;
            }

            return new LocalizedString(_tableReference, key);
        }
    }
}