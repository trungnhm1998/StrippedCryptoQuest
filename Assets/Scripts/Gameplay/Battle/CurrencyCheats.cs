using CommandTerminal;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.System;
using CryptoQuest.System.Cheat;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class CurrencyCheats : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private ServiceProvider _serviceProvider;
        [SerializeField] private CurrencySO _goldSo;

        public void InitCheats()
        {
            Debug.Log("EncounterCheats::InitCheats()");
            Terminal.Shell.AddCommand("addgold", AddGold, 1, 1, "Add gold to player");
        }

        private void AddGold(CommandArg[] args)
        {
            float amount = args[0].Float;
            CurrencyInfo goldInfo = new(_goldSo, amount);
            CurrencyLootInfo goldLootInfo = new(goldInfo);
            goldLootInfo.AddItemToInventory(_serviceProvider.Inventory);
        }
    }
}