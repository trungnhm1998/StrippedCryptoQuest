using CryptoQuest.UI.Dialogs.RewardDialog;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace CryptoQuest.Inventory.ConfigureEquipmentRewardDisplays
{
    public abstract class ConfigureEquipmentRewardDisplay<T> : SagaBase<T> where T : RequestConfigEquipmentRewardBase
    {
        [SerializeField] private EquipmentIdToName _database;
        [SerializeField] private TableReference _tableReference;


        protected override void HandleAction(T ctx)
        {
            var key = GetStringKey(ctx.Loot.EquipmentId, _database);
            if (string.IsNullOrEmpty(key)) return;

            LocalizedString localizedString = new LocalizedString(_tableReference, key);
            ctx.RewardItem.SetContentStringRef(localizedString);
        }

        private string GetStringKey(string id, EquipmentIdToName database)
        {
            foreach (var param in database.Params)
            {
                if (param.equipment_id != id) continue;
                return param.localize_key;
            }

            return "";
        }
    }
}