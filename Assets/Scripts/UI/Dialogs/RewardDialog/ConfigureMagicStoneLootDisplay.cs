using CryptoQuest.Mappings;
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
        [SerializeField] private NameMappingDatabase _magicStoneDatabase;
        [SerializeField] private TableReference _tableReference;
        [SerializeField] private LocalizedString _itemWithQuantity;

        protected override void HandleAction(ConfigureMagicStoneLootDisplayAction ctx)
        {
            if (!IsCorrectStoneSetup(ctx)) return;

            var msg = new LocalizedString();
            msg.SetReference(_itemWithQuantity.TableReference, _itemWithQuantity.TableEntryReference);
            msg.Add("item", GetMagicStoneString(ctx.Loot.StoneId));
            msg.Add("quantity", new IntVariable { Value = ctx.Loot.Quantity });

            ctx.UIRewardItem.SetContentStringRef(msg);
        }

        private bool IsCorrectStoneSetup(ConfigureMagicStoneLootDisplayAction ctx)
        {
            if (ctx.Loot.Quantity <= 0) return false;
            var mappings = _magicStoneDatabase.NameMappings;
            foreach (var mapping in mappings)
            {
                if (mapping.Id == ctx.Loot.StoneId) return true;
            }

            return false;
        }

        private LocalizedString GetMagicStoneString(string id)
        {
            var key = "";
            foreach (var magicStone in _magicStoneDatabase.NameMappings)
            {
                if (magicStone.Id != id) continue;
                key = magicStone.Name;
            }

            LocalizedString localizedString = new LocalizedString();
            localizedString.SetReference(_tableReference, key);

            return localizedString;
        }
    }
}