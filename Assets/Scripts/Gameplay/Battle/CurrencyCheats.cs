using CommandTerminal;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.System;
using CryptoQuest.System.Cheat;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class CurrencyCheats : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private CurrencySO _goldSo;

        public void InitCheats()
        {
            Debug.Log("EncounterCheats::InitCheats()");
            Terminal.Shell.AddCommand("addgold", AddGold, 1, 1, "Add gold to player");
        }

        private void AddGold(CommandArg[] args)
        {
            float amount = args[0].Float;
            new CurrencyInfo(_goldSo, amount).AddToInventory(ServiceProvider.GetService<IInventoryController>());
        }
    }
}