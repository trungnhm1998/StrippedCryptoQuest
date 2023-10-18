using System.Collections;
using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.AbilitySystem.Executions;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Battle.Core.Helper;
using CryptoQuest.Tests.Runtime.Battle.Builder;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using CharacterComponent = CryptoQuest.Battle.Components.Character;

namespace CryptoQuest.Tests.Runtime.Battle
{
    public class CommandTests : BattleFixtureBase
    {
        private GameObject _heroGo;
        private CharacterComponent _hero;
        private GameObject _enemyGo;
        private CharacterComponent _enemy;

        [SetUp]
        public void Setup()
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
        }

        [TestFixture]
        public class Attack : CommandTests
        {
            [Test]
            public void Execute_FireHeroAttackWaterEnemy_DamageGreaterThan15()
            {
                _enemy.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var enemyHPBefore);
                var command = new NormalAttackCommand(_hero, _enemy);
                command.Execute();

                _enemy.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var enemyHPAfter);
                Assert.Greater(enemyHPBefore.CurrentValue - enemyHPAfter.CurrentValue, 15f);
            }

            [Test]
            public void Execute_FireHeroAttackFireEnemy_DamageGreaterThan22()
            {
                _enemy.Init(An.Element.Fire);
                _enemy.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var enemyHPBefore);
                var command = new NormalAttackCommand(_hero, _enemy);
                command.Execute();

                _enemy.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var enemyHPAfter);
                Assert.Greater(enemyHPBefore.CurrentValue - enemyHPAfter.CurrentValue, 22f);
            }
        }

        [TestFixture]
        public class Guard : CommandTests
        {
            /// <summary>
            /// Water attack Fire should cause 30 damage
            ///
            /// Guard should reduce damage by half
            /// </summary>
            [Test]
            public void Execute_HeroGuardEnemyAttack_DamageShouldHalves()
            {
                _hero.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var hpBefore);

                var commands = new List<ICommand>
                {
                    new GuardCommand(_hero.GetComponent<HeroBehaviour>()),
                    new NormalAttackCommand(_enemy, _hero)
                };
                commands.ForEach(command => command.Execute());

                _hero.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var hpAfter);
                var damage = hpBefore.CurrentValue - hpAfter.CurrentValue;

                // should be around 15f
                Assert.Greater(damage, 10f);
                Assert.Less(damage, 20f);
            }
        }

        [TestFixture]
        public class Retreat : CommandTests
        {
            [TestCase(50f, 50f, 50f)]
            [TestCase(50f, 200f, 125f)]
            public void CalculateProbabilityOfRetreat(float enemy, float hero, float expected)
            {
                var actual = BattleCalculator.CalculateProbabilityOfRetreat(enemy, hero);
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void Execute_PlayerHaveHigherAgilityThanEnemies_RetreatSuccess()
            {
                var retreatAbility =
                    AssetDatabase.LoadAssetAtPath<RetreatAbility>(
                        "Assets/ScriptableObjects/Character/Abilities/GA_Retreat.asset");

                bool retreated = false;
                retreatAbility.RetreatFailedEvent += (AbilitySystemBehaviour owner) => retreated = false;
                retreatAbility.RetreatedEvent += (AbilitySystemBehaviour owner) => retreated = true;

                var hero = A.Character
                    .WithStats(new AttributeWithValue[]
                    {
                        new(AttributeSets.Agility, 200f)
                    })
                    .Build();
                hero.gameObject.AddComponent<RetreatBehaviour>();
                var retreatBehaviour = hero.GetComponent<RetreatBehaviour>();
                retreatBehaviour.Editor_SetAbility(retreatAbility);
                retreatBehaviour.Init();

                var enemyBuilder = A.Character;
                var enemy = enemyBuilder
                    .WithStats(new AttributeWithValue[]
                    {
                        new(AttributeSets.Agility, 50f)
                    })
                    .Build();
                var enemy2 = enemyBuilder
                    .WithStats(new AttributeWithValue[]
                    {
                        new(AttributeSets.Agility, 150f)
                    })
                    .Build();

                var enemies = new List<CharacterComponent>() { enemy, enemy2 };
                var highestAgi = enemies.GetHighestAttributeValue<CharacterComponent>(AttributeSets.Agility);

                Assert.AreEqual(150f, highestAgi);

                var commands = new List<ICommand>
                {
                    new RetreatCommand(hero.GetComponent<HeroBehaviour>(), highestAgi)
                };
                foreach (var command in commands)
                {
                     command.Execute();
                }

                Assert.IsTrue(retreated);

                // Maybe command could play effect/logs?
                // async?
                // Assert.IsTrue(commands.TrueForAll(command => command.IsDone));
            }

            [Test]
            public void Execute_PlayerHaveLowerAgilityThanOneEnemy_RetreatFailed()
            {
                var retreatAbility =
                    AssetDatabase.LoadAssetAtPath<RetreatAbility>(
                        "Assets/ScriptableObjects/Character/Abilities/GA_Retreat.asset");

                bool retreated = false;
                retreatAbility.RetreatFailedEvent += (AbilitySystemBehaviour owner) => retreated = false;
                retreatAbility.RetreatedEvent += (AbilitySystemBehaviour owner) => retreated = true;

                var characterBuilder = A.Character;
                var hero = characterBuilder
                    .WithStats(new AttributeWithValue[]
                    {
                        new(AttributeSets.Agility, 50f)
                    })
                    .Build();
                hero.gameObject.AddComponent<RetreatBehaviour>();
                var retreatBehaviour = hero.GetComponent<RetreatBehaviour>();
                retreatBehaviour.Editor_SetAbility(retreatAbility);
                retreatBehaviour.Init();

                var enemyBuilder = A.Character;
                var enemy = enemyBuilder
                    .WithStats(new AttributeWithValue[]
                    {
                        new(AttributeSets.Agility, 25f)
                    })
                    .Build();
                var enemy2 = enemyBuilder
                    .WithStats(new AttributeWithValue[]
                    {
                        new(AttributeSets.Agility, 150f)
                    })
                    .Build();

                var enemies = new List<CharacterComponent>() { enemy, enemy2 };
                var highestAgi = enemies.GetHighestAttributeValue<CharacterComponent>(AttributeSets.Agility);
                var commands = new List<ICommand>
                {
                    new RetreatCommand(hero.GetComponent<HeroBehaviour>(), highestAgi)
                };
                foreach (var command in commands)
                {
                    command.Execute();
                }

                Assert.IsFalse(retreated);
            }
        }

        [TestFixture]
        public class ConsumeItem { }

        [TestFixture]
        public class CastSkill { }
    }
}