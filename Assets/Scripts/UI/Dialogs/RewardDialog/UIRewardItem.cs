using CryptoQuest.Gameplay.Loot;
using IndiGames.Core.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.UI.Dialogs.RewardDialog
{
    public class UIRewardItem : MonoBehaviour
    {
        [SerializeField] private LocalizedString _itemWithQuantity;
        [SerializeField] private LocalizeStringEvent _content;
        [SerializeField] private TMP_Text _text;

        public void SetLoot(LootInfo loot)
        {
            loot.Accept(this);
        }

        public void Visit(ConsumableLootInfo loot)
        {
            var msg = new LocalizedString(_itemWithQuantity.TableReference, _itemWithQuantity.TableEntryReference)
            {
                {
                    "item", loot.Item.Data.DisplayName
                },
                {
                    "quantity", new IntVariable { Value = loot.Item.Quantity }
                }
            };
            _content.StringReference = msg;
        }

        public void Visit(CurrencyLootInfo loot)
        {
            _text.text = $"{loot.Item.Amount} {loot.Item.Data.DisplayName.GetLocalizedString()}";
        }

        public void Visit(EquipmentLoot loot)
        {
            Debug.LogWarning($"Try to loot equipment loot {loot.EquipmentId} but haven't implemented yet");
            ActionDispatcher.Dispatch(new RequestConfigEquipmentRewardInfo(this, loot));
        }

        public void Visit(ExpLoot loot) => _text.text = $"{loot.Exp} EXP";
        public void SetContentStringRef(LocalizedString localizedString) => _content.StringReference = localizedString;
    }

    public class RequestConfigEquipmentRewardInfo : ActionBase
    {
        public UIRewardItem RewardItem { get; set; }
        public EquipmentLoot Loot { get; set; }

        public RequestConfigEquipmentRewardInfo(UIRewardItem rewardItem, EquipmentLoot loot)
        {
            RewardItem = rewardItem;
            Loot = loot;
        }
    }

    public class RequestConfigEquipmentRewardBase : ActionBase
    {
        public UIRewardItem RewardItem { get; set; }
        public EquipmentLoot Loot { get; set; }

        public RequestConfigEquipmentRewardBase(UIRewardItem rewardItem, EquipmentLoot loot)
        {
            RewardItem = rewardItem;
            Loot = loot;
        }
    }

    public class RequestConfigWeaponReward : RequestConfigEquipmentRewardBase
    {
        public RequestConfigWeaponReward(UIRewardItem rewardItem, EquipmentLoot loot) : base(rewardItem, loot) { }
    }

    public class RequestConfigArmorReward : RequestConfigEquipmentRewardBase
    {
        public RequestConfigArmorReward(UIRewardItem rewardItem, EquipmentLoot loot) : base(rewardItem, loot) { }
    }

    public class RequestConfigAccessoryReward : RequestConfigEquipmentRewardBase
    {
        public RequestConfigAccessoryReward(UIRewardItem rewardItem, EquipmentLoot loot) : base(rewardItem, loot) { }
    }
}