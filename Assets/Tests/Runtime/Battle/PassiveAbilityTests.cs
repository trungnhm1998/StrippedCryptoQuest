using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.AbilitySystem.Executions;
using CryptoQuest.Battle.ScriptableObjects;
using CryptoQuest.Tests.Runtime.Battle.Builder;
using CryptoQuestEditor.Gameplay.Gameplay.Abilities;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.Battle
{
    public class PassiveAbilityTests
    {
        [TestFixture]
        public class CastPassiveSkill : CastMagicAbilityTests.CastSkillTest
        {
            private const string PASSIVE_ABILITY_FOLDER =
                "Assets/ScriptableObjects/Character/Skills/Passive/";

            private const string MAPPING_PATH =
                "Assets/ScriptableObjects/AbilitySystem/Abilities/AbilityAssetMappingEditor.asset";

            private AbilityAssetMappingEditor _mapping;

            [UnityTest]
            public IEnumerator ExecuteAbility_ReturnCorrectValueOrCorrectTag(
                [ValueSource("TestObject")] string abilityName)
            {
                var stats = new AttributeWithValue[]
                {
                    new(AttributeSets.MaxHealth, 100f),
                    new(AttributeSets.Health, 100f),
                    new(AttributeSets.Strength, 50f),
                    new(AttributeSets.MagicAttack, 50f),
                    new(AttributeSets.Intelligence, 50f),
                    new(AttributeSets.MaxMana, 100f),
                    new(AttributeSets.Mana, 100f),
                    new(AttributeSets.Attack, 100f),
                    new(AttributeSets.Defense, 100f),
                    new(AttributeSets.Agility, 100f),
                    new(AttributeSets.Vitality, 100f),
                    new(AttributeSets.CriticalRate, 1f),
                    new(AttributeSets.FireAttack, 100f),
                    new(AttributeSets.WaterAttack, 100f),
                    new(AttributeSets.EarthAttack, 100f),
                    new(AttributeSets.WindAttack, 100f),
                    new(AttributeSets.WoodAttack, 100f),
                    new(AttributeSets.LightAttack, 100f),
                    new(AttributeSets.DarkAttack, 100f),
                    new(AttributeSets.FireResist, 100f),
                    new(AttributeSets.WaterResist, 100f),
                    new(AttributeSets.EarthResist, 100f),
                    new(AttributeSets.WindResist, 100f),
                    new(AttributeSets.WoodResist, 100f),
                    new(AttributeSets.LightResist, 100f),
                    new(AttributeSets.DarkResist, 100f),
                };
                _hero = A.Character
                    .WithStats(stats)
                    .WithElement(An.Element.Fire).Build();
                _enemy = A.Character
                    .WithStats(stats)
                    .WithElement(An.Element.Fire).Build();

                string path = GetPathByName(abilityName);

                LoadMappings(MAPPING_PATH);
                var handle = LoadAbilityInstance(path);
                yield return handle;

                var ability = handle.Current.Result;
                switch (ability.Context.SkillInfo.SkillParameters.EffectType)
                {
                    case EEffectType.Buff:
                        TestBuffAbility(ability);
                        break;
                    case EEffectType.Nullify:
                        TestImmuneAbility(ability);
                        break;

                    default:
                        Assert.Pass("Wrong effect type");
                        break;
                }
            }

            private void TestBuffAbility(PassiveAbility ability)
            {
                var skillInfo = ability.Context.SkillInfo;
                var targetedAttribute = skillInfo.SkillParameters.TargetAttribute.Attribute;

                _hero.AttributeSystem.SetAttributeBaseValue(targetedAttribute, 100f);
                var baseValue =
                    BattleCalculator.CalculateMagicSkillBasePower(ability.Context.SkillInfo.SkillParameters, 50f);
                _hero.AttributeSystem.TryGetAttributeValue(targetedAttribute, out var targetedAttributeBefore);
                var expectedLower = 0f;
                var expectedUpper = 0f;
                if (skillInfo.SkillParameters.IsFixed)
                {
                    expectedLower = baseValue + baseValue * (-0.05f);
                    expectedUpper = baseValue + baseValue * (0.05f);
                }
                else
                {
                    expectedLower = Mathf.RoundToInt((baseValue + baseValue * (-0.05f))) *
                        targetedAttributeBefore.CurrentValue / 100;

                    expectedUpper = Mathf.RoundToInt((baseValue + baseValue * (0.05f))) *
                        targetedAttributeBefore.CurrentValue / 100;
                }


                var spec = _hero.AbilitySystem.GiveAbility<PassiveWithEffectAbilitySpec>(ability);
                _hero.AttributeSystem.TryGetAttributeValue(targetedAttribute, out var targetedAttributeAfter);
                Debug.Log(
                    $"Before: {targetedAttributeBefore.CurrentValue} After: {targetedAttributeAfter.CurrentValue} " +
                    $"Upper: {expectedUpper} Lower: {expectedLower}");

                Assert.LessOrEqual(
                    MathF.Round((targetedAttributeAfter.CurrentValue - targetedAttributeBefore.CurrentValue), 1),
                    MathF.Round(expectedUpper, 1));
                Assert.GreaterOrEqual(
                    MathF.Round((targetedAttributeAfter.CurrentValue - targetedAttributeBefore.CurrentValue), 1),
                    MathF.Round(expectedLower, 1));
            }

            private void TestImmuneAbility(PassiveAbility ability)
            {
                var spec = _hero.AbilitySystem.GiveAbility<PassiveAbilitySpec>(ability);
                GameplayEffectDefinition abnormalEffect = null;
                foreach (var effectMap in _mapping.GameEffectMaps)
                {
                    var abilityTags = ability.Tags.ActivationTags.ToList();
                    var effectTags = effectMap.Value.ApplicationTagRequirements.IgnoreTags.ToList();
                    bool isValid = ContainsAllTag(effectTags, abilityTags);
                    if (!isValid) continue;
                    abnormalEffect = effectMap.Value;
                    break;
                }

                if (abnormalEffect == null) Assert.Fail("Cannot find effect");

                var effectSpec = _hero.GameplayEffectSystem.GetEffect(abnormalEffect);
                _hero.GameplayEffectSystem.ApplyEffectToSelf(effectSpec);

                foreach (var tag in abnormalEffect.GrantedTags)
                {
                    bool hasTag = _hero.HasTag(tag);
                    Assert.True(hasTag);
                }
            }

            private bool ContainsAllTag(List<TagScriptableObject> a, List<TagScriptableObject> b)
            {
                return !b.Except(a).Any();
            }

            private IEnumerator<AsyncOperationHandle<PassiveAbility>> LoadAbilityInstance(string path)
            {
                string guid = AssetDatabase.AssetPathToGUID(path);
                var handle = Addressables.LoadAssetAsync<PassiveAbility>(guid);
                yield return handle;
            }

            private string GetPathByName(string name) => $"{PASSIVE_ABILITY_FOLDER}{name}.asset";


            static IEnumerable<string> TestObject()
            {
                var castSkillAbilities = Directory.GetFiles(PASSIVE_ABILITY_FOLDER);
                foreach (var abilityAddress in castSkillAbilities)
                {
                    if (abilityAddress.Contains(".meta")) continue;
                    string nameAsset = abilityAddress.Split("/")[^1];
                    string name = nameAsset.Split(".")[0];
                    yield return name;
                }
            }

            private void LoadMappings(string path)
            {
                _mapping = AssetDatabase.LoadAssetAtPath<AbilityAssetMappingEditor>(path);
            }
        }
    }
}