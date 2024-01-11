using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Dialogs.RewardDialog;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace CryptoQuest.Inventory
{
    public class ConfigureEquipmentRewardDisplay : SagaBase<RequestConfigEquipmentRewardInfo>
    {
        [SerializeField] private EquipmentIdToName _accessoryDatabase;
        [SerializeField] private EquipmentIdToName _armorDatabase;
        [SerializeField] private EquipmentIdToName _weaponDatabase;
        [SerializeField] private TableReference _accessoryTableReference;
        [SerializeField] private TableReference _armorTableReference;
        [SerializeField] private TableReference _weaponTableReference;


        protected override void HandleAction(RequestConfigEquipmentRewardInfo ctx)
        {
            var categoryIndex = GetCategoryIndex(ctx.Loot.EquipmentId);
            var database = GetDatabase(categoryIndex);
            var key = GetStringKey(ctx.Loot.EquipmentId, database);
            var tableReference = GetTableReference(categoryIndex);
            if (string.IsNullOrEmpty(key)) return;

            LocalizedString localizedString = new LocalizedString(tableReference, key);
            ctx.RewardItem.SetContentStringRef(localizedString);
        }

        private string GetStringKey(string id, EquipmentIdToName database)
        {
            foreach (var param in database.param)
            {
                if (param.equipment_id != id) continue;
                return param.localize_key;
            }

            return "";
        }

        private int GetCategoryIndex(string id) => int.Parse(id[2].ToString()) - 1;

        private TableReference GetTableReference(int categoryIndex)
        {
            switch ((EEquipmentCategory)categoryIndex)
            {
                case EEquipmentCategory.Weapon:
                    return _weaponTableReference;
                case EEquipmentCategory.Accessory:
                    return _accessoryTableReference;
                default:
                    return _armorTableReference;
            }
        }

        private EquipmentIdToName GetDatabase(int categoryIndex)
        {
            switch ((EEquipmentCategory)categoryIndex)
            {
                case EEquipmentCategory.Weapon:
                    return _weaponDatabase;
                case EEquipmentCategory.Accessory:
                    return _accessoryDatabase;
                default:
                    return _armorDatabase;
            }
        }
    }
}