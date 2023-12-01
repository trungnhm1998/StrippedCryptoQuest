using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Actions;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using APIProfile = CryptoQuest.API.Profile;

namespace CryptoQuest.Sagas.Profile
{
    public class FetchProfileEquipments : SagaBase<FetchProfileEquipmentsAction>
    {
        [SerializeField] private EquipmentPrefabDatabase _prefabDatabase;
        [SerializeField] private GetEquipmentsEvent _inGameEquipmentsUpdate;
        [SerializeField] private PassiveAbilityDatabase _passiveAbilityDatabase;
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

            var equipments = new List<NftEquipment>();
            foreach (var equipmentResponse in responseEquipments)
            {
                yield return _prefabDatabase.LoadDataByIdAsync(equipmentResponse.equipmentIdForeign);
                var equipment = CreateNftEquipment(equipmentResponse,
                    _prefabDatabase.GetDataById(equipmentResponse.equipmentIdForeign));
                equipments.Add(equipment);
            }

            _inventory.NftEquipments.Clear();
            _inventory.NftEquipments.AddRange(equipments);
            ActionDispatcher.Dispatch(new InventoryFilled());
        }

        private NftEquipment CreateNftEquipment(EquipmentResponse equipmentResponse, EquipmentPrefab prefab)
        {
            var nftEquipment = new NftEquipment
            {
                Id = equipmentResponse.id,
                TokenId = equipmentResponse.equipmentTokenId,
                Level = equipmentResponse.lv
            };
            FillEquipmentData(equipmentResponse, ref nftEquipment, prefab);
            return nftEquipment;
        }

        private void FillEquipmentData(EquipmentResponse response, ref NftEquipment nftEquipment,
            EquipmentPrefab prefab)
        {
            nftEquipment.Data = new EquipmentData()
            {
                ID = response.equipmentId,
                Prefab = prefab,
                Rarity = _rarities.FirstOrDefault(rarity => rarity.ID == response.rarityId),
                Stars = response.star,
                RequiredCharacterLevel = response.restrictedLv,
                MinLevel = response.minLv,
                MaxLevel = response.maxLv,
                ValuePerLvl = response.valuePerLv,
                Stats = GetStats(response)
            };
            StartCoroutine(FillEquipmentSkills(response, nftEquipment));
        }

        private IEnumerator FillEquipmentSkills(EquipmentResponse response, NftEquipment nftEquipment)
        {
            var skills = new List<int>(response.conditionSkills);
            skills.AddRange(response.passiveSkills);

            var passiveList = new List<PassiveAbility>();
            foreach (var skillId in skills)
            {
                yield return _passiveAbilityDatabase.LoadDataById(skillId);
                passiveList.Add(_passiveAbilityDatabase.GetDataById(skillId));
            }

            nftEquipment.Data.Passives = passiveList.ToArray();
        }

        private AttributeWithValue[] GetStats(EquipmentResponse equipmentResponse)
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

            return stats.ToArray();
        }

        private void OnError(Exception error)
        {
            Debug.Log("FetchProfileEquipments::OnError " + error);
        }

        private void OnCompleted() { }
    }
}