using System.Collections.Generic;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.ExecutionCalculations;
using CryptoQuest.Character.Ability;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Tests.Runtime.Battle.Builder;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Runtime.Battle
{
    public class CommandTests : BattleFixtureBase
    {
        private GameObject _heroGo;
        private CryptoQuest.Battle.Components.Character _hero;
        private GameObject _enemyGo;
        private CryptoQuest.Battle.Components.Character _enemy;

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
        public class Escape : CommandTests
        {
            [TestCase(50f, 50f, 50f)]
            [TestCase(50f, 200f, 125f)]
            public void CalculateProbabilityOfRetreat(float enemy, float hero, float expected)
            {
                var actual = BattleCalculator.CalculateProbabilityOfRetreat(enemy, hero);
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void Execute_PlayerHaveHigherAgilityThanEnemy_EscapeSuccess()
            {
                var escapeAbility =
                    AssetDatabase.LoadAssetAtPath<EscapeAbility>(
                        "Assets/ScriptableObjects/Character/Abilities/GA_Escape.asset");

                bool escaped = false;
                escapeAbility.EscapeFailedEvent += () => escaped = false;
                escapeAbility.EscapedEvent += () => escaped = true;

                var hero = A.Character
                    .WithStats(new AttributeWithValue[]
                    {
                        new(AttributeSets.Agility, 200f)
                    })
                    .Build();
                var enemyBuilder = A.Character;
                var enemy = enemyBuilder
                    .WithStats(new AttributeWithValue[]
                    {
                        new(AttributeSets.Agility, 50f)
                    })
                    .Build();

                enemy.AttributeSystem.TryGetAttributeValue(AttributeSets.Agility, out var enemyAgility);
                var commands = new List<ICommand>
                {
                    new EscapeCommand(enemyBuilder.CharacterGameObject, enemyAgility.CurrentValue)
                };
                commands.ForEach(command => command.Execute());

                Assert.IsTrue(escaped);

                // Maybe command could play effect/logs?
                // async?
                // Assert.IsTrue(commands.TrueForAll(command => command.IsDone));
            }

            [Test]
            public void Execute_EnemyHas200AgiHeroHas50_EscapeFailed()
            {
                var escapeAbility =
                    AssetDatabase.LoadAssetAtPath<EscapeAbility>(
                        "Assets/ScriptableObjects/Character/Abilities/GA_Escape.asset");

                bool escaped = false;
                escapeAbility.EscapeFailedEvent += () => escaped = false;
                escapeAbility.EscapedEvent += () => escaped = true;

                var characterBuilder = A.Character;
                var hero = characterBuilder
                    .WithStats(new AttributeWithValue[]
                    {
                        new(AttributeSets.Agility, 50f)
                    })
                    .Build();
                var enemy = A.Character
                    .WithStats(new AttributeWithValue[]
                    {
                        new(AttributeSets.Agility, 200f)
                    })
                    .Build();


                enemy.AttributeSystem.TryGetAttributeValue(AttributeSets.Agility, out var enemyAgility);
                var commands = new List<ICommand>
                {
                    new EscapeCommand(characterBuilder.CharacterGameObject, enemyAgility.CurrentValue)
                };
                commands.ForEach(command => command.Execute());

                Assert.IsFalse(escaped);
            }
        }

        [TestFixture]
        public class ConsumeItem { }

        [TestFixture]
        public class CastSkill { }
    }
}