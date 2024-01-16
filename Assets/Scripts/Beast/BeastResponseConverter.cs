using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Gameplay;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Beast
{
    public interface IBeastResponseConverter
    {
        IBeast Convert(BeastResponse responseObject);
    }

    public class BeastResponseConverter : MonoBehaviour, IBeastResponseConverter
    {
        [SerializeField] private PassiveAbilityDatabase _passiveAbilityDatabase;
        [SerializeField] private BeastDefinitionDatabase _database;

        private void Awake() => ServiceProvider.Provide<IBeastResponseConverter>(this);

        public IBeast Convert(BeastResponse response)
        {
            var beast = new Beast()
            {
                Id = response.id,
                BeastId = response.beastId,
                Level = response.level,
                MaxLevel = response.maxLv,
                Stars = response.star,
                Elemental = _database.GetElemental(response.elementId),
                Class = _database.GetClass(response.classId),
                Type = _database.GetType(response.characterId),
                Stats = FillBeastStats(response)
            };

            StartCoroutine(CoLoadPassiveAsync(beast, response));
            return beast;
        }

        private IEnumerator CoLoadPassiveAsync(Beast beast, BeastResponse response)
        {
            var passiveHandler = _passiveAbilityDatabase.LoadDataById(response.passiveSkillId);
            yield return passiveHandler;

            beast.Passive = _passiveAbilityDatabase.GetDataById(response.passiveSkillId);
        }

        private StatsDef FillBeastStats(BeastResponse response)
        {
            var initialAttributes = new Dictionary<AttributeScriptableObject, CappedAttributeDef>();
            foreach (var fieldInfo in _database.GetFields())
            {
                if (!_database.TryGetAttribute(fieldInfo.Name, out var attribute)) continue;

                var value = (float)fieldInfo.GetValue(response);
                if (initialAttributes.TryGetValue(attribute, out var def))
                {
                    if (fieldInfo.Name.Contains("min"))
                        def.MinValue = value;
                    else
                        def.MaxValue = value;
                    initialAttributes[attribute] = def;
                }
                else
                {
                    initialAttributes.Add(attribute, new CappedAttributeDef(attribute)
                    {
                        MinValue = fieldInfo.Name.Contains("min") ? value : -1,
                        MaxValue = fieldInfo.Name.Contains("max") ? value : -1
                    });
                }
            }

            var stats = new StatsDef
            {
                MaxLevel = response.maxLv,
                Attributes = initialAttributes.Values.ToArray()
            };
            return stats;
        }
    }
}