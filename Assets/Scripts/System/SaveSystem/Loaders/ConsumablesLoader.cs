using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.API;
using CryptoQuest.Inventory;
using CryptoQuest.Item.Consumable;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using UniRx;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class ConsumablesLoader : Loader
    {
        [SerializeField] private ConsumableInventory _inventory;
        [SerializeField] private ConsumableSO[] _consumables;

        private readonly Dictionary<string, ConsumableSO> _consumableMap = new();
        private bool _inventoryFilled;

        public override IEnumerator LoadAsync()
        {
            foreach (var consumable in _consumables)
            {
                _consumableMap.TryAdd(consumable.ID, consumable);
            }

            _inventory.Items.Clear();
            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .Get<ItemsResponse>(Consumable.ITEMS)
                .ToYieldInstruction();
            yield return op;

            var itemResponses = op.Result.data.items;
            foreach (var item in itemResponses)
            {
                if (item.itemNum <= 0) continue;
                _inventory.Items.Add(new ConsumableInfo(_consumableMap[item.itemId], item.itemNum));
            }

        }
    }

    #region Network Object

    [Serializable]
    public class ItemsResponse : CommonResponse
    {
        public Data data;

        [Serializable]
        public class Data
        {
            public ItemResponse[] items;
        }
    }

    [Serializable]
    public class ItemResponse
    {
        public string itemId;
        public int itemNum;
        public int itemType;
        public string nameJp;
        public string descriptionEn;
        public string descriptionJp;
        public int effectType;
        public int valueType;
        public string localizeKey;
        public int skillType;
        public int categoryId;
        public int targetId;
        public int amountUsed;
        public int targetParameterId;
        public string targetParameter;
        public int effectTriggerTiming;
        public int continousTurn;
        public int basePower;
        public int successRate;
        public int usageScenario;
        public int price;
        public int sellingPrice;
    }

    #endregion
}