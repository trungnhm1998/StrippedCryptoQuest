using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Actions;
using CryptoQuest.API;
using CryptoQuest.Beast;
using CryptoQuest.Character;
using CryptoQuest.Networking;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
using TinyMessenger;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Obj = CryptoQuest.Sagas.Objects;


namespace CryptoQuest.Ranch.Sagas
{
    public class FetchProfileBeast : MonoBehaviour
    {
        [SerializeField] private AssetReferenceT<BeastInventorySO> _beastInventoryAsset;
        [SerializeField] private List<Elemental> _elements = new();
        [SerializeField] private List<CharacterClass> _classes = new();
        [SerializeField] private List<BeastTypeSO> _type = new();
        [SerializeField] private List<PassiveAbility> _passive = new();
        private BeastInventorySO _beastInventory;
        private TinyMessageSubscriptionToken _fetchEvent;
        private TinyMessageSubscriptionToken _transferSuccessEvent;

        private void OnEnable()
        {
            _fetchEvent = ActionDispatcher.Bind<FetchProfileBeastAction>(HandleAction);
            _transferSuccessEvent = ActionDispatcher.Bind<TransferSucceed>(FilterAndRefreshInventory);
        }

        private void FilterAndRefreshInventory(TransferSucceed ctx)
        {
            var ingameBeasts = ctx.ResponseBeasts.Where(beast => beast.inGameStatus == (int)Obj.EBeastStatus.InGame)
                .ToList();
            OnInventoryFilled(ingameBeasts.ToArray());
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_fetchEvent);
            ActionDispatcher.Unbind(_transferSuccessEvent);
        }

        private void HandleAction(FetchProfileBeastAction _)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();

            restClient
                .WithParams(new Dictionary<string, string>
                    { { "source", $"{((int)Obj.EBeastStatus.InGame).ToString()}" } })
                .Get<Obj.BeastsResponse>(Profile.GET_BEASTS)
                .Subscribe(ProcessResponseBeasts, OnError);
        }

        private void ProcessResponseBeasts(Obj.BeastsResponse obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            if (obj.code != (int)HttpStatusCode.OK) return;

            var responseBeasts = obj.data.beasts;
            if (responseBeasts.Length == 0) return;

            OnInventoryFilled(responseBeasts);
        }

        private void OnInventoryFilled(Obj.BeastData[] beasts) => StartCoroutine(CoLoadAndUpdateInventory(beasts));

        private IEnumerator CoLoadAndUpdateInventory(Obj.BeastData[] beasts)
        {
            if (_beastInventory == null)
            {
                var handle = _beastInventoryAsset.LoadAssetAsync();
                yield return handle;
                _beastInventory = handle.Result;
            }

            var nftBeast = beasts.Select(CreateNftBeast).ToList();
            _beastInventory.OwnedBeasts.Clear();
            _beastInventory.OwnedBeasts = nftBeast;
            Debug.Log($"CoLoadAndUpdateInventory: {nftBeast}");
        }

        private Beast.Beast CreateNftBeast(Obj.BeastData beastResponse)
        {
            Debug.Log($"CreateNftBeast: {beastResponse}");
            return FillBeastData(beastResponse);
        }

        private void OnError(Exception error)
        {
            Debug.LogError($"FetchProfileBeast::OnError: {error}");
        }

        private Beast.Beast FillBeastData(Obj.BeastData response)
        {
            return new Beast.Beast
            {
                Id = response.id,
                Elemental = _elements.FirstOrDefault(element => element.Id == Int32.Parse(response.elementId)),
                Class = _classes.FirstOrDefault(classes => classes.Id == Int32.Parse(response.classId)),
                Type =
                    _type.FirstOrDefault(type => type.BeastInformation.Id == Int32.Parse(response.characterId)),
                Passive = _passive.FirstOrDefault(passive => passive.Id == response.passiveSkillId)
            };
        }
    }
}