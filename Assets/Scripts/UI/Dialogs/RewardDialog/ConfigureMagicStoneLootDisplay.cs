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
            if (!IsCorrectStoneSetup(ctx)) return;

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

        private bool IsCorrectStoneSetup(ConfigureMagicStoneLootDisplayAction ctx)
        {
            if (ctx.Loot.Quantity <= 0) return false;
            var stones = _magicStoneDatabase.sheets[0].list;
            foreach (var stone in stones)
            {
                if (stone.stone_id == ctx.Loot.StoneId) return true;
            }

            return false;
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