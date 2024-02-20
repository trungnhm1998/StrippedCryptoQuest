using System;
using System.Collections;
using CryptoQuest.API;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Item.Consumable;
using CryptoQuest.Item.MagicStone.Sagas;
using CryptoQuest.Networking;
using CryptoQuest.Quest.Sagas;
using CryptoQuest.Sagas.Equipment;
using CryptoQuest.Sagas.Items;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Inventory
{
    public class QuestFinishRequestHandler : SagaBase<QuestFinishRequestAction>
    {
        private struct Body
        {
            [JsonProperty("questId")] public string QuestName { get; set; }
        }

        private IRestClient _restClient;

        protected override void HandleAction(QuestFinishRequestAction ctx)
        {
            _restClient = ServiceProvider.GetService<IRestClient>();
            _restClient.WithoutDispactError()
                .WithBody(new Body() { QuestName = ctx.QuestName })
                .Post<RewardResponse>(Quests.QUEST_FINISH)
                .Subscribe(OnSucceed, OnFailed);
        }

        private void OnSucceed(RewardResponse response)
        {
            AddGold(response.data.rewardGold);
            AddDiamond(response.data.rewardMetad);
            AddExp(response.data.rewardExp);
            
            if (response.data.rewards == null) return;
            var rewards = response.data.rewards;

            AddEquipments(rewards.equipments);
            AddItems(rewards.items);
            AddStones(rewards.stones);
        }

        private void AddGold(int gold)
        {
            if (gold == 0) return;
            ActionDispatcher.Dispatch(new AddGoldAction(gold));
        }

        private void AddDiamond(int diamond)
        {
            if (diamond == 0) return;
            ActionDispatcher.Dispatch(new AddDiamonds(diamond));
        }

        private void AddExp(int exp)
        {
            if (exp == 0) return;
            ActionDispatcher.Dispatch(new AddExpToPartyAction(exp));
        }

        private void AddEquipments(EquipmentResponse[] equipments)
        {
            if (equipments.Length == 0) return;
            var equipmentConverter = ServiceProvider.GetService<IEquipmentResponseConverter>();
            foreach (var equipmentResponse in equipments)
            {
                var equipment = equipmentConverter.Convert(equipmentResponse);
                ActionDispatcher.Dispatch(new AddEquipmentAction(equipment));
            }
        }

        private void AddItems(RewardResponse.Items[] items)
        {
            if (items.Length == 0) return;
            var consumableConverter = ServiceProvider.GetService<IConsumableResponseConverter>();
            foreach (var itemResponse in items)
            {
                var consumableInfo = consumableConverter.Convert(itemResponse);
                StartCoroutine(CoAddConsumable(consumableInfo));
            }
        }

        private void AddStones(MagicStone[] stones)
        {
            if (stones.Length == 0) return;
            foreach (var stone in stones)
            {
                var stoneConverter = ServiceProvider.GetService<IMagicStoneResponseConverter>();
                if (stone.inGameStatus != 2) continue;
                var magicStone = stoneConverter.Convert(stone);
                ActionDispatcher.Dispatch(new AddStoneAction(magicStone));
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


        private void OnFailed(Exception exception)
        {
            Debug.LogWarning($"Quest finish fail. Log:\n{exception}");
        }
    }
}