using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Components;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Tests.Runtime.Character;
using CryptoQuest.UI.Utilities;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;

namespace CryptoQuest.Tests.Runtime.Item.MagicStone
{
    [TestFixture]
    public class MagicStonePassiveControllerTests : HeroTestFixture
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            Hero.AddComponent<PassivesController>();

            SetAttribute(AttributeSets.Strength, 100);
            SetAttribute(AttributeSets.Attack, 100);
            SetAttribute(AttributeSets.Intelligence, 100);
            SetAttribute(AttributeSets.MagicAttack, 100);
        }

        [Test]
        public void ApplyPassive_SameStoneTwice_ShouldStackEffect()
        {
            var passive =
                AssetDatabase.LoadAssetAtPath<PassiveAbility>(
                    "Assets/ScriptableObjects/Character/Skills/Passive/4001.asset");
            var stone1 = Substitute.For<IMagicStone>();
            stone1.Passives.Returns(new[] { passive });

            var stone2 = Substitute.For<IMagicStone>();
            stone2.Passives.Returns(new[] { passive });

            var controller = Hero.GetOrAddComponent<MagicStonePassiveController>();

            controller.ApplyPassives(stone1);
            controller.ApplyPassives(stone2);

            Assert.AreEqual(2, AbilitySystem.GrantedAbilities.Count);
            Assert.AreEqual(2, EffectSystem.AppliedEffects.Count);
            var magnitude = EffectSystem.AppliedEffects[0].ComputedModifiers[0].Magnitude;
            AttributeSystem.TryGetAttributeValue(AttributeSets.Attack, out var atk);
            Assert.AreEqual(magnitude * 2, atk.ExternalModifier.Multiplicative);
        }
    }
}