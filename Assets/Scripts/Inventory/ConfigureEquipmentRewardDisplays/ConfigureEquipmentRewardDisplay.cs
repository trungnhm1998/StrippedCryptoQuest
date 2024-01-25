using CryptoQuest.Mappings;
using CryptoQuest.UI.Dialogs.RewardDialog;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace CryptoQuest.Inventory.ConfigureEquipmentRewardDisplays
{
    public abstract class ConfigureEquipmentRewardDisplay<T> : SagaBase<T> where T : RequestConfigEquipmentRewardBase
    {
        [SerializeField] private NameMappingDatabase _database;
        [SerializeField] private TableReference _tableReference;


        protected override void HandleAction(T ctx)
        {
            var key = GetStringKey(ctx.Loot.EquipmentId, _database);
            if (string.IsNullOrEmpty(key)) return;

            LocalizedString localizedString = new LocalizedString();
            localizedString.SetReference(_tableReference, key);
            ctx.RewardItem.SetContentStringRef(localizedString);
        }

        private string GetStringKey(string id, NameMappingDatabase database)
        {
            foreach (var mapping in database.NameMappings)
            {
                if (mapping.Id != id) continue;
                return mapping.Name;
            }

            return "";
        }
    }
}