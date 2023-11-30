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
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TestTools;
using CharacterComponent = CryptoQuest.Battle.Components.Character;

namespace CryptoQuest.Tests.Runtime.Battle
{
    public class CastMagicAbilityTests
    {
        public class CastSkillTest
        {
            protected GameObject _heroGo;
            protected CharacterComponent _hero;
            protected GameObject _enemyGo;
            protected CharacterComponent _enemy;
            protected Dictionary<int, CastSkillAbility> _castEffectsOnTargetAbilities = new();
            public static List<int> _sourceList = new();
            protected static List<CastSkillAbility> _castSkillAbilities = new();
            protected static int[] _castSkillAbilitiesArray = new[] { 1001, 1002 };

            protected const string MAGIC_ABILITY_FOLDER =
                "Assets/ScriptableObjects/Character/Skills/Castables/Magical/";

            [UnitySetUp]
            public IEnumerator Setup()
            {
                var stats = new AttributeWithValue[]
                {
                    new(AttributeSets.MaxHealth, 100f),
                    new(AttributeSets.Health, 100f),
                    new(AttributeSets.Strength, 50f),
                };
                _hero = A.Character
                    .WithStats(stats)
                    .WithElement(An.Element.Fire).Build();
                _enemy = A.Character
                    .WithStats(stats)
                    .WithElement(An.Element.Water).Build();

                _castEffectsOnTargetAbilities.Clear();
                yield return null;
            }
        }

        [TestFixture]
        public class CastDamageSkill : CastSkillTest
        {
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
                var handle = LoadAbilityInstance(path);
                yield return handle;
                var ability = handle.Current.Result;
                switch (ability.Context.SkillInfo.SkillParameters.EffectType)
                {
                    case EEffectType.Damage:
                        TestDamageAbility(ability);
                        break;
                    case EEffectType.Buff:
                        TestBuffAbility(ability);
                        break;
                    case EEffectType.DeBuff:
                        TestDebuffAbility(ability);
                        break;
                    case EEffectType.AddStateChange:
                        TestAbnormalAbility(ability);
                        break;
                    case EEffectType.RemoveAbnormalStatus:
                        TestRemoveAbnormalAbility(ability);
                        break;
                    case EEffectType.Nullify:
                        TestImmuneAbility(ability);
                        break;

                    default:
                        Assert.Fail("Wrong effect type");
                        break;
                }
            }

            private void TestDamageAbility(CastSkillAbility ability)
            {
                CastEffectsOnTargetAbility castedAbility = ability as CastEffectsOnTargetAbility;
                var baseValue =
                    BattleCalculator.CalculateMagicSkillBasePower(ability.Context.SkillInfo.SkillParameters, 50f);
                var effectSpec = castedAbility.Effects[0].CreateEffectSpec(_hero.AbilitySystem,
                    new GameplayEffectContextHandle(castedAbility.Context));

                effectSpec.Source = _hero.AbilitySystem;
                effectSpec.Target = _enemy.AbilitySystem;
                CustomExecutionParameters customExecutionParameters = new CustomExecutionParameters(effectSpec);
                float elementalRate = BattleCalculator.CalculateElementalRateFromParams(customExecutionParameters);

                var expectedLower = baseValue + baseValue * (-0.05f);
                var expectedUpper = baseValue + baseValue * (0.05f);
                expectedLower *= elementalRate;
                expectedUpper *= elementalRate;
                _enemy.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var enemyHPBefore);


                var spec = _hero.AbilitySystem.GiveAbility<CastEffectsOnTargetAbilitySpec>(ability);
                spec.Execute(_enemy.AbilitySystem);
                _enemy.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var enemyHPAfter);
                Assert.LessOrEqual(enemyHPBefore.CurrentValue - enemyHPAfter.CurrentValue, expectedUpper);
                Assert.GreaterOrEqual(enemyHPBefore.CurrentValue - enemyHPAfter.CurrentValue, expectedLower);
            }

            private void TestBuffAbility(CastSkillAbility ability)
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


                var spec = _hero.AbilitySystem.GiveAbility<CastEffectsOnTargetAbilitySpec>(ability);
                spec.Execute(_hero.AbilitySystem);
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

            private void TestDebuffAbility(CastSkillAbility ability)
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
                    expectedLower = expectedUpper = baseValue * targetedAttributeBefore.CurrentValue / 100;
                }


                var spec = _hero.AbilitySystem.GiveAbility<CastEffectsOnTargetAbilitySpec>(ability);
                spec.Execute(_hero.AbilitySystem);
                _hero.AttributeSystem.TryGetAttributeValue(targetedAttribute, out var targetedAttributeAfter);
                Debug.Log(
                    $"Before: {targetedAttributeBefore.CurrentValue} After: {targetedAttributeAfter.CurrentValue} " +
                    $"Upper: {expectedUpper} Lower: {expectedLower}");
                Assert.LessOrEqual(
                    MathF.Round((targetedAttributeBefore.CurrentValue - targetedAttributeAfter.CurrentValue), 2),
                    MathF.Round(expectedUpper, 2));
                Assert.GreaterOrEqual(
                    MathF.Round((targetedAttributeBefore.CurrentValue - targetedAttributeAfter.CurrentValue), 2),
                    MathF.Round(expectedLower, 2));
            }

            private void TestAbnormalAbility(CastSkillAbility ability)
            {
                CastEffectsOnTargetAbility castedAbility = ability as CastEffectsOnTargetAbility;

                if (castedAbility == null) Assert.Fail("Wrong ability set up scriptable object");

                var effects = castedAbility.Effects;
                List<TagScriptableObject> tags = new List<TagScriptableObject>();
                foreach (var effect in effects)
                {
                    tags.AddRange(effect.GrantedTags.ToList());
                }

                CastEffectsOnTargetAbility clone = ScriptableObject.Instantiate(castedAbility);
                SerializedObject so = new SerializedObject(clone);
                so.FindProperty("<SuccessRate>k__BackingField").floatValue = 100;
                so.ApplyModifiedProperties();
                so.Update();
                var spec = _hero.AbilitySystem.GiveAbility<CastEffectsOnTargetAbilitySpec>(clone);
                spec.Execute(_enemy.AbilitySystem);
                foreach (var tag in tags)
                {
                    bool hasTag = _enemy.HasTag(tag);
                    Assert.True(hasTag);
                }
            }

            private void TestRemoveAbnormalAbility(CastSkillAbility ability)
            {
                CastRemoveAbnormalAbility castedAbility = ability as CastRemoveAbnormalAbility;

                if (castedAbility == null) Assert.Fail("Wrong ability set up scriptable object");

                var effects = castedAbility.Effects;
                List<TagScriptableObject> tags = new List<TagScriptableObject>();
                foreach (var tag in castedAbility.CancelEffectWithTags)
                {
                    tags.Add(tag);
                }

                CastRemoveAbnormalAbility clone = ScriptableObject.Instantiate(castedAbility);
                SerializedObject so = new SerializedObject(clone);
                so.FindProperty("<SuccessRate>k__BackingField").floatValue = 100;
                so.ApplyModifiedProperties();
                so.Update();
                var spec = _hero.AbilitySystem.GiveAbility<CastEffectsOnTargetAbilitySpec>(clone);
                spec.Execute(_enemy.AbilitySystem);
                foreach (var tag in tags)
                {
                    bool hasTag = _enemy.HasTag(tag);
                    Assert.False(hasTag);
                }
            }

            private void TestImmuneAbility(CastSkillAbility ability)
            {
                CastEffectsOnTargetAbility castedAbility = ability as CastEffectsOnTargetAbility;

                if (castedAbility == null) Assert.Fail("Wrong ability set up scriptable object");

                var effects = castedAbility.Effects;
                List<TagScriptableObject> tags = new List<TagScriptableObject>();
                foreach (var effect in effects)
                {
                    tags.AddRange(effect.GrantedTags.ToList());
                }

                CastEffectsOnTargetAbility clone = ScriptableObject.Instantiate(castedAbility);
                SerializedObject so = new SerializedObject(clone);
                so.FindProperty("<SuccessRate>k__BackingField").floatValue = 100;
                so.ApplyModifiedProperties();
                so.Update();
                var spec = _hero.AbilitySystem.GiveAbility<CastEffectsOnTargetAbilitySpec>(clone);
                spec.Execute(_enemy.AbilitySystem);
                foreach (var tag in tags)
                {
                    bool hasTag = _enemy.HasTag(tag);
                    Assert.True(hasTag);
                }
            }

            private IEnumerator<AsyncOperationHandle<CastSkillAbility>> LoadAbilityInstance(string path)
            {
                string guid = AssetDatabase.AssetPathToGUID(path);
                var handle = Addressables.LoadAssetAsync<CastSkillAbility>(guid);
                yield return handle;
            }

            private string GetPathByName(string name) => $"{MAGIC_ABILITY_FOLDER}{name}.asset";


            static IEnumerable<string> TestObject()
            {
                var castSkillAbilities = Directory.GetFiles(MAGIC_ABILITY_FOLDER);
                foreach (var abilityAddress in castSkillAbilities)
                {
                    if (abilityAddress.Contains(".meta")) continue;
                    string nameAsset = abilityAddress.Split("/")[^1];
                    string name = nameAsset.Split(".")[0];
                    yield return name;
                }
            }
        }
    }
}