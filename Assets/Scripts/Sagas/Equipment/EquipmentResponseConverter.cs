using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Sagas.Equipment
{
    public interface IEquipmentResponseConverter
    {
        IEquipment Convert(EquipmentResponse response);
    }

    public class EquipmentResponseConverter : MonoBehaviour, IEquipmentResponseConverter
    {
        [SerializeField] private EquipmentPrefabDatabase _prefabDatabase;
        [SerializeField] private PassiveAbilityDatabase _passiveAbilityDatabase;
        [SerializeField] private EquipmentsMinStatsSO _equipmentsMinStatsSO;

        /// <summary>
        /// Map the response attribute name to the attribute scriptable object
        /// </summary>
        [SerializeField] private List<ResponseAttributeMap> _attributeMap = new();

        [SerializeField] private List<RaritySO> _rarities = new();

        private Dictionary<string, AttributeScriptableObject> _lookupAttribute = new();
        private Dictionary<string, AttributeScriptableObject> _lookupMinAttribute = new();
        private FieldInfo[] _fields;

        private void Awake()
        {
            _lookupAttribute = _attributeMap.ToDictionary(map => map.Name, map => map.Attribute);

            foreach (var attribute in _attributeMap)
            {
                _lookupMinAttribute.TryAdd($"min{attribute.Name.ToLower()}", attribute.Attribute);
                _lookupMinAttribute.TryAdd($"add{attribute.Name.ToLower()}", attribute.Attribute);
            }
            _fields = typeof(EquipmentResponse).GetFields();
            ServiceProvider.Provide<IEquipmentResponseConverter>(this);
        }

        public IEquipment Convert(EquipmentResponse response)
        {
            IEquipment equipment = new Item.Equipment.Equipment()
            {
                // TokenId = equipmentResponse.equipmentTokenId,
                Id = response.id,
                Level = response.lv,
                Data = ConvertData(response),
                IsNft = response.nft == 1
            };

            return equipment;
        }

        private EquipmentData ConvertData(EquipmentResponse response)
        {
            var data = new EquipmentData()
            {
                ID = response.equipmentId,
                Rarity = _rarities.FirstOrDefault(rarity => rarity.ID == response.rarityId),
                Stars = response.star,
                RequiredCharacterLevel = response.requiredLv,
                MinLevel = response.minLv,
                MaxLevel = response.maxLv,
                ValuePerLvl = response.valuePerLv,
                Stats = GetStats(response),
                StoneSlots = response.slot,
                AttachStones = response.attachStones.Select(stone => stone.id).ToList()
            };

            _equipmentsMinStatsSO.EquipmentsMinStats.TryAdd(response.id, GetMinStats(data, response));

            _prefabDatabase.LoadDataByIdAsync(response.equipmentIdForeign).Completed += op =>
            {
                data.Prefab = op.Result;
            };

            StartCoroutine(LoadEquipmentSkills(response, data));

            return data;
        }

        private AttributeWithValue[] GetStats(EquipmentResponse equipmentResponse)
        {
            var stats = new List<AttributeWithValue>();
            // using reflection here, might optimize if this hits performance
            foreach (var fieldInfo in _fields)
            {
                if (_lookupAttribute.TryGetValue(fieldInfo.Name, out var attributeSO) == false) continue;
                var value = (float)fieldInfo.GetValue(equipmentResponse);
                if (value <= 0) continue;
                stats.Add(new AttributeWithValue(attributeSO, value));
            }

            return stats.ToArray();
        }

        private AttributeWithValue[] GetMinStats(EquipmentData data, EquipmentResponse equipmentResponse)
        {
            var stats = new List<AttributeWithValue>();
            foreach (var fieldInfo in _fields)
            {
                if (_lookupMinAttribute.TryGetValue(fieldInfo.Name.ToLower(), out var attributeSO) == false) continue;
                var value = (float)fieldInfo.GetValue(equipmentResponse);
                if (value <= 0) continue;
                var existStats = stats.FirstOrDefault(stat => stat.Attribute == attributeSO);
                if (existStats.Attribute != null)
                {
                    stats.Remove(existStats);
                    existStats.Value += value;
                    stats.Add(new AttributeWithValue(attributeSO, existStats.Value));
                    continue;
                }

                stats.Add(new AttributeWithValue(attributeSO, value));
            }

            return stats.ToArray();
        }

        private IEnumerator LoadEquipmentSkills(EquipmentResponse response, EquipmentData data)
        {
            var skills = new List<int>(response.conditionSkills);
            skills.AddRange(response.passiveSkills);

            var passiveList = new List<PassiveAbility>();
            foreach (var skillId in skills)
            {
                yield return _passiveAbilityDatabase.LoadDataById(skillId);
                passiveList.Add(_passiveAbilityDatabase.GetDataById(skillId));
            }

            data.Passives = passiveList.ToArray();
        }
    }
}