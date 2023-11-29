using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Actions;
using CryptoQuest.API;
using CryptoQuest.Character;
using CryptoQuest.Character.Beast;
using CryptoQuest.Core;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using TinyMessenger;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static CryptoQuest.Character.Beast.BeastSpec;
using Obj = CryptoQuest.Sagas.Objects;
using Spec = CryptoQuest.Character.Beast;


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
            var ingameBeasts = ctx.ResponseBeasts.Where(beast => beast.inGameStatus == (int)EBeastStatus.InGame)
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
                    { { "source", $"{((int)EBeastStatus.InGame).ToString()}" } })
                .Get<BeastsResponse>(Profile.GET_BEASTS)
                .Subscribe(ProcessResponseBeasts, OnError);
        }

        private void ProcessResponseBeasts(BeastsResponse obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            if (obj.code != (int)HttpStatusCode.OK) return;

            var responseBeasts = obj.data.beasts;
            if (responseBeasts.Length == 0) return;

            OnInventoryFilled(responseBeasts);
        }

        private void OnInventoryFilled(Obj.Beast[] beasts) => StartCoroutine(CoLoadAndUpdateInventory(beasts));

        private IEnumerator CoLoadAndUpdateInventory(Obj.Beast[] beasts)
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

        private Spec.Beast CreateNftBeast(Obj.Beast beast)
        {
            var nftBeast = new Spec.Beast();
            FillBeastData(beast, ref nftBeast);
            Debug.Log($"CreateNftBeast: {beast}");
            return nftBeast;
        }

        private void OnError(Exception error)
        {
            Debug.LogError($"FetchProfileBeast::OnError: {error}");
        }

        private void FillBeastData(Obj.Beast response, ref Spec.Beast beast)
        {
            var spec = new BeastSpec();
            beast.Id = response.id;
            spec.Elemental = _elements.FirstOrDefault(element => element.Id == Int32.Parse(response.elementId));
            spec.Class = _classes.FirstOrDefault(classes => classes.Id == Int32.Parse(response.classId));
            spec.BeastTypeSo = _type.FirstOrDefault(type => type.BeastInformation.Id == Int32.Parse(response.characterId));
            spec.Passives = _passive.FirstOrDefault(passive => passive.Id == response.passiveSkillId);
            beast.Data = spec;
        }
    }
}