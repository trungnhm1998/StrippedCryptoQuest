using System;
using CommandTerminal;
using CryptoQuest.Networking;
using CryptoQuest.System.Cheat;
using CryptoQuest.UI.Actions;
using UnityEngine;
using UniRx;
using Newtonsoft.Json;
using System.Collections.Generic;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.API;
using CryptoQuest.Inventory.Currency;
using CryptoQuest.Inventory.ScriptableObjects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;

namespace CryptoQuest.Gameplay.Battle
{
    public class CurrencyCheats : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _goldSo;
        [SerializeField] private CurrencySO _diamondSo;

        private IRestClient _restClient;

        private void OnEnable()
        {
            _restClient = ServiceProvider.GetService<IRestClient>();
        }

        public void InitCheats()
        {
            Terminal.Shell.AddCommand("add.gold", AddGold, 1, 1, 
                "add.gold <amount_to_add> add gold to server");
            Terminal.Shell.AddCommand("add.diamond", AddDiamond, 1, 1, 
                "add.diamond <amount_to_add> add diamond to server");
        }

        private void AddGold(CommandArg[] args)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            _restClient
                .WithBody(new GoldBody { Gold = args[0].Int })
                .WithHeaders(new Dictionary<string, string> { { "DEBUG_KEY", Profile.DEBUG_KEY } })
                .Post<ProfileResponse>(Cheats.ADD_GOLD)
                .Subscribe(OnAddSucceed, OnAddFailed);
        }

        private void AddDiamond(CommandArg[] args)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            _restClient
                .WithBody(new DiamondBody { Diamond = args[0].Int })
                .WithHeaders(new Dictionary<string, string> { { "DEBUG_KEY", Profile.DEBUG_KEY },
                    {
                        "Content-Type",
                        "application/json"
                    } })
                .Post<ProfileResponse>(Cheats.ADD_DIAMOND)
                .Subscribe(OnAddSucceed, OnAddFailed);
        }

        private void OnAddFailed(Exception exception)
        {
            Debug.LogWarning($"Add currency fail. Log:\n{exception.Message}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnAddSucceed(ProfileResponse response)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            Debug.LogWarning($"Add currency succeed.");
            _wallet[_goldSo].SetAmount(response.gold);
            _wallet[_diamondSo].SetAmount(response.diamond);
        }
        
        [Serializable]
        public struct GoldBody
        {
            [JsonProperty("gold")]
            public int Gold;
        }

        [Serializable]
        public struct DiamondBody
        {
            [JsonProperty("diamond")]
            public float Diamond;
        }
    }
}