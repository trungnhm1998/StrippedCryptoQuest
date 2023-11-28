using CommandTerminal;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.System.Cheat;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class CurrencyCheats : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _goldSo;

        public void InitCheats()
        {
            Debug.Log("EncounterCheats::InitCheats()");
            Terminal.Shell.AddCommand("addgold", AddGold, 1, 1, "Add gold to player");
        }

        private void AddGold(CommandArg[] args)
        {
            float amount = args[0].Float;
            _wallet[_goldSo].SetAmount(_wallet[_goldSo].Amount + amount);
        }
    }
}