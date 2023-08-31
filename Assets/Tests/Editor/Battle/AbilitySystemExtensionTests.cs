using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using CryptoQuest.Gameplay.Battle.Helper;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using System.Linq;
using CryptoQuest.Gameplay.Battle.Core;
using CryptoQuest.Gameplay;

namespace CryptoQuest.Tests.Editor.Battle
{
    public class AbilitySystemExtensionTests
    {
        private const string SCENARIO_DIRECTORY = "Assets/ScriptableObjects/Battle/Skills/SkillRelatedData/UsageScenario/";
        private const string FIELD_SCENARIO = SCENARIO_DIRECTORY + "Field.asset";
        private const string BATTLE_SCENARIO = SCENARIO_DIRECTORY + "Battle.asset";
        private const string FIELD_AND_BATTLE_SCENARIO = SCENARIO_DIRECTORY + "FieldAndBattle.asset";

        private GameObject _abilityOwner;
        private AbilitySystemBehaviour _abilitySystem;
        private SimpleAbilitySO _fieldAbilitySO;
        private SimpleAbilitySO _battleAbilitySO;
        private SimpleAbilitySO _fieldAndBattleAbilitySO;
        private SimpleGameplayAbilitySpec _fieldAbility;
        private SimpleGameplayAbilitySpec _battleAbility;
        private SimpleGameplayAbilitySpec _fieldAndBattleAbility;

        [SetUp]
        public void Setup()
        {
            _abilityOwner = new GameObject();
            _abilitySystem = _abilityOwner.AddComponent<AbilitySystemBehaviour>();
            var skillInfoBuilder = new SkillInfoBuilder();

            _fieldAbilitySO = ScriptableObject.CreateInstance<SimpleAbilitySO>();
            // TODO: REFACTOR BATTLE
            // _fieldAbilitySO.Editor_SetInfo(skillInfoBuilder
            //     .WithUsageScenario(AssetDatabase.LoadAssetAtPath<AbilityUsageScenarioSO>(FIELD_SCENARIO))
            //     .Build());

            _battleAbilitySO = ScriptableObject.CreateInstance<SimpleAbilitySO>();
            // _battleAbilitySO.Editor_SetInfo(skillInfoBuilder
            //     .WithUsageScenario(AssetDatabase.LoadAssetAtPath<AbilityUsageScenarioSO>(BATTLE_SCENARIO))
            //     .Build());

            _fieldAndBattleAbilitySO = ScriptableObject.CreateInstance<SimpleAbilitySO>();
            // _fieldAndBattleAbilitySO.Editor_SetInfo(skillInfoBuilder
            //     .WithUsageScenario(AssetDatabase.LoadAssetAtPath<AbilityUsageScenarioSO>(FIELD_AND_BATTLE_SCENARIO))
            //     .Build());

            _fieldAbility = _abilitySystem.GiveAbility(_fieldAbilitySO) as SimpleGameplayAbilitySpec;
            _battleAbility = _abilitySystem.GiveAbility(_battleAbilitySO) as SimpleGameplayAbilitySpec;
            _fieldAndBattleAbility = _abilitySystem.GiveAbility(_fieldAndBattleAbilitySO) as SimpleGameplayAbilitySpec;
        }

        [Test]
        public void GetAbilitiesInBattle_ShouldContainBattleAbility()
        {
            var abilities = _abilitySystem.GetAbilitiesInBattle();
            Assert.IsTrue(abilities.Contains(_battleAbility));
            Assert.IsTrue(abilities.Contains(_fieldAndBattleAbility));
            Assert.AreEqual(2, abilities.Count());
        }

        [Test]
        public void GetAbilitiesInField_ShouldContainFieldAbility()
        {
            var abilities = _abilitySystem.GetAbilitiesInField();
            Assert.IsTrue(abilities.Contains(_fieldAbility));
            Assert.IsTrue(abilities.Contains(_fieldAndBattleAbility));
            Assert.AreEqual(2, abilities.Count());
        }

        [Test]
        public void GetAbilitiesInFieldAndBattle_ShouldContainFieldAndBattleAbility()
        {
            var abilities = _abilitySystem.GetAbilitiesInBattleAndField();
            Assert.IsTrue(abilities.Contains(_fieldAndBattleAbility));
            Assert.AreEqual(1, abilities.Count());
        }
    }
}
