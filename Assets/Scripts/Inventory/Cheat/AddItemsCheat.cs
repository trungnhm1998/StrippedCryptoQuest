using System;
using System.Collections;
using CommandTerminal;
using CryptoQuest.API;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Item.Consumable;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Items;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System.Cheat;
using CryptoQuest.System.SaveSystem.Loaders;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Inventory.Cheat
{
    public class AddItemsCheat : MonoBehaviour, ICheatInitializer
    {
        [Serializable]
        public struct Body
        {
            [JsonProperty("itemId")]
            public string ItemId;

            [JsonProperty("itemNum")]
            public string ItemNumber;
        }

        private int _quantity;

        public void InitCheats()
        {
            Debug.Log("AddMaterialToChangeClassCheat::InitCheats()");
            Terminal.Shell.AddCommand("add.items", AddItems, 2, 2, "<Items ID> <Quantity>: Add Items to inventory");
        }

        private void AddItems(CommandArg[] args)
        {
            StartCoroutine(CreateItems(args));
        }

        private IEnumerator CreateItems(CommandArg[] args)
        {
            _quantity = args[1].Int;
            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .WithBody(new Body { ItemId = args[0].String, ItemNumber = args[1].String })
                .Post<ItemsResponse>(Cheats.ADD_ITEMS)
                .ToYieldInstruction();
            yield return op;
            var itemResponses = op.Result.data.items;
            ConvertResponse(itemResponses);
        }

        private void ConvertResponse(ItemResponse[] itemResponses)
        {
            var consumableConverter = ServiceProvider.GetService<IConsumableResponseConverter>();
            foreach (var itemResponse in itemResponses)
            {
                RewardResponse.Items item = new RewardResponse.Items()
                {
                    itemId = itemResponse.itemId,
                    itemNum = _quantity
                };
                var consumableInfo = consumableConverter.Convert(item);
                StartCoroutine(CoAddConsumable(consumableInfo));
            }
        }

        private IEnumerator CoAddConsumable(ConsumableInfo info)
        {
            while (info.Data == null)
            {
                yield return null;
            }

            ActionDispatcher.Dispatch(new AddConsumableAction(info.Data, info.Quantity));
        }
    }
}