using System;
using System.Collections.Generic;
using CryptoQuest.Actions;
using CryptoQuest.API;
using CryptoQuest.Inventory;
using CryptoQuest.Item.Consumable;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public class FetchConsumableInventorySaga : SagaBase<FetchProfileConsumablesAction>
    {
        [SerializeField] private ConsumableInventory _inventory;
        [SerializeField] private ConsumableSO[] _consumables;
        
        private readonly Dictionary<string, ConsumableSO> _consumableMap = new();

#if UNITY_EDITOR
        private void OnValidate()
        {
            var consumableAssetGuids = AssetDatabase.FindAssets("t:ConsumableSO");
            _consumables = new ConsumableSO[consumableAssetGuids.Length];
            for (var index = 0; index < consumableAssetGuids.Length; index++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(consumableAssetGuids[index]);
                _consumables[index] = AssetDatabase.LoadAssetAtPath<ConsumableSO>(assetPath);
            }
        }
#endif

        private void Awake()
        {
            foreach (var consumable in _consumables)
            {
                _consumableMap.Add(consumable.ID, consumable);
            }
        }

        protected override void HandleAction(FetchProfileConsumablesAction ctx)
        {
            _inventory.Items.Clear();
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Get<ItemsResponse>(Consumable.ITEMS)
                .Subscribe(AddToInventory);
        }

        private void AddToInventory(ItemsResponse response)
        {
            var responseItems = response.data.items;
            foreach (var item in responseItems)
            {
                _inventory.Items.Add(new ConsumableInfo(_consumableMap[item.itemId], item.itemNum));
            }
        }
    }

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
}