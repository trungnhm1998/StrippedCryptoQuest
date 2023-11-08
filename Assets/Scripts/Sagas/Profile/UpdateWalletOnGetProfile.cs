using System;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Networking;
using CryptoQuest.Networking.Actions;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Sagas.Profile
{
    public class UpdateWalletOnGetProfile : SagaBase<GetProfileSucceed>
    {
        [SerializeField] private Credentials _credentials;
        [SerializeField] private WalletSO _wallet;

        [SerializeField] private CurrencySO _gold;
        [SerializeField] private CurrencySO _soul;
        [SerializeField] private CurrencySO _metad;

        protected override void HandleAction(GetProfileSucceed ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient.Get<ProfileResponse>(Networking.API.Profile.GET_PROFILE)
                .Subscribe(UpdateProfileInfo, OnError);
        }

        private void UpdateProfileInfo(ProfileResponse response)
        {
            _credentials.Profile.user.walletAddress = response.data.walletAddress;
            _wallet[_gold].SetAmount(response.gold);
            _wallet[_soul].SetAmount(response.soul);
            _wallet[_metad].SetAmount(response.diamond);
        }

        private void OnError(Exception response)
        {
            Debug.LogError($"UpdateWalletOnGetProfile::OnError [{response}]");
        }
    }
}