using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using CryptoQuest.Actions;
using CryptoQuest.Core;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using APIProfile = CryptoQuest.API.Profile;

namespace CryptoQuest.Sagas.Profile
{
    public class FetchProfileEquipments : SagaBase<FetchProfileEquipmentsAction>
    {
        [SerializeField] private GetEquipmentsEvent _inGameEquipmentsUpdate;
        [SerializeField] private AssetReferenceT<InventorySO> _inventoryAsset;
        private InventorySO _inventory;

        /// <summary>
        /// Map the response attribute name to the attribute scriptable object
        /// </summary>
        [SerializeField] private List<ResponseAttributeMap> _attributeMap = new();

        [SerializeField] private List<RaritySO> _rarities = new();

        private Dictionary<string, AttributeScriptableObject> _lookupAttribute = new();
        private FieldInfo[] _fields;

        private void Awake()
        {
            _lookupAttribute = _attributeMap.ToDictionary(map => map.Name, map => map.Attribute);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _inGameEquipmentsUpdate.EventRaised += RefreshInventoryWithOnline;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _inGameEquipmentsUpdate.EventRaised -= RefreshInventoryWithOnline;
        }

        private void RefreshInventoryWithOnline(List<EquipmentResponse> nftEquipmentsResponse)
            => OnInventoryFilled(nftEquipmentsResponse.ToArray());

        protected override void HandleAction(FetchProfileEquipmentsAction _)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithParams(new Dictionary<string, string>
                    { { "source", $"{((int)EEquipmentStatus.InGame).ToString()}" } })
                .Get<EquipmentsResponse>(APIProfile.EQUIPMENTS)
                .Subscribe(ProcessResponseEquipments, OnError, OnCompleted);
        }

        private void ProcessResponseEquipments(EquipmentsResponse response)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            if (response.code != (int)HttpStatusCode.OK) return;
            var responseEquipments = response.data.equipments;
            if (responseEquipments.Length == 0) return;
            OnInventoryFilled(responseEquipments);
        }

        private void OnInventoryFilled(EquipmentResponse[] responseEquipments)
            => StartCoroutine(CoLoadAndUpdateInventory(responseEquipments));

        private IEnumerator CoLoadAndUpdateInventory(EquipmentResponse[] responseEquipments)
        {
            if (_inventory == null)
            {
                var handle = _inventoryAsset.LoadAssetAsync();
                yield return handle;
                _inventory = handle.Result;
            }

            var nftEquipments = responseEquipments.Select(CreateNftEquipment).ToList();
            _inventory.NftEquipments.Clear();
            _inventory.NftEquipments = nftEquipments;
            ActionDispatcher.Dispatch(new InventoryFilled());
        }

        private NftEquipment CreateNftEquipment(EquipmentResponse equipmentResponse)
        {
            var nftEquipment = new NftEquipment(equipmentResponse.id);
            FillEquipmentData(equipmentResponse, ref nftEquipment);
            return nftEquipment;
        }

        private void FillEquipmentData(EquipmentResponse response, ref NftEquipment nftEquipment)
        {
            nftEquipment.Data = new EquipmentData();
            FillEquipmentStats(response, ref nftEquipment);
            nftEquipment.Data.PrefabId = response.equipmentIdForeign;
            nftEquipment.Level = response.lv;
            nftEquipment.Data.Rarity = _rarities.FirstOrDefault(rarity => rarity.ID == response.rarityId);
            nftEquipment.Data.ID = response.equipmentId;
            nftEquipment.Data.Stars = response.star;
            nftEquipment.Data.RequiredCharacterLevel = response.restrictedLv;
            nftEquipment.Data.MinLevel = 1; // TODO: IMPLEMENT
            nftEquipment.Data.MaxLevel = response.maxLv;
            nftEquipment.TokenId = response.equipmentTokenId;
            nftEquipment.Data.ValuePerLvl = response.valuePerLv;
        }

        private void FillEquipmentStats(EquipmentResponse equipmentResponse, ref NftEquipment nftEquipment)
        {
            var stats = new List<AttributeWithValue>();
            // using reflection here, might optimize if this hits performance
            _fields ??= typeof(EquipmentResponse).GetFields();
            foreach (var fieldInfo in _fields)
            {
                if (_lookupAttribute.TryGetValue(fieldInfo.Name, out var attributeSO) == false) continue;
                var value = (float)fieldInfo.GetValue(equipmentResponse);
                if (value <= 0) continue;
                stats.Add(new AttributeWithValue(attributeSO, value));
            }

            if (stats.Count == 0) return;
            nftEquipment.Data.Stats = stats.ToArray();
        }

        private void OnError(Exception error)
        {
            Debug.Log("FetchProfileEquipments::OnError " + error);
        }

        private void OnCompleted() { }
    }
}