using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Dialogs.RewardDialog;
using IndiGames.Core.Events;

namespace CryptoQuest.Inventory.ConfigureEquipmentRewardDisplays
{
    public class DisplayConfigureRequestDispatcher : SagaBase<RequestConfigEquipmentRewardInfo>
    {
        protected override void HandleAction(RequestConfigEquipmentRewardInfo ctx)
        {
            var categoryIndex = GetCategoryIndex(ctx.Loot.EquipmentId);
            DispatchRequest(categoryIndex, ctx);
        }

        private int GetCategoryIndex(string id) => int.Parse(id[2].ToString()) - 1;

        private void DispatchRequest(int categoryIndex, RequestConfigEquipmentRewardInfo ctx)
        {
            switch ((EEquipmentCategory)categoryIndex)
            {
                case EEquipmentCategory.Weapon:
                    ActionDispatcher.Dispatch(new RequestConfigWeaponReward(ctx.RewardItem, ctx.Loot));
                    break;
                case EEquipmentCategory.Accessory:
                    ActionDispatcher.Dispatch(new RequestConfigAccessoryReward(ctx.RewardItem, ctx.Loot));
                    break;
                default:
                    ActionDispatcher.Dispatch(new RequestConfigArmorReward(ctx.RewardItem, ctx.Loot));
                    break;
            }
        }
    }
}