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

namespace CryptoQuest.Sagas.Profile
{
    public interface IEquipmentResponseConverter
    {
        IEquipment Convert(EquipmentResponse response);
    }

    public class EquipmentResponseConverter : MonoBehaviour, IEquipmentResponseConverter
    {
        [SerializeField] private EquipmentPrefabDatabase _prefabDatabase;
        [SerializeField] private PassiveAbilityDatabase _passiveAbilityDatabase;

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
            _fields = typeof(EquipmentResponse).GetFields();
            ServiceProvider.Provide<IEquipmentResponseConverter>(this);
        }

        public IEquipment Convert(EquipmentResponse response)
        {
            IEquipment equipment = new Equipment()
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
                RequiredCharacterLevel = response.restrictedLv,
                MinLevel = response.minLv,
                MaxLevel = response.maxLv,
                ValuePerLvl = response.valuePerLv,
                Stats = GetStats(response)
            };

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