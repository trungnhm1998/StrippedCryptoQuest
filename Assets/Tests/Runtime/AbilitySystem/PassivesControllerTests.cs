using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Tests.Runtime.Character;
using NUnit.Framework;
using UnityEditor;

namespace CryptoQuest.Tests.Runtime.AbilitySystem
{
    [TestFixture]
    public class PassivesControllerTests : HeroTestFixture
    {
        private const string PASSIVE_4001 = "Assets/ScriptableObjects/Character/Skills/Passive/4001.asset";
        private const string CONDITIONAL_3001 = "Assets/ScriptableObjects/Character/Skills/Conditionals/3001.asset";

        private PassiveAbility _passive4001;
        private PassiveAbility _conditional3001;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            _passive4001 = AssetDatabase.LoadAssetAtPath<PassiveAbility>(PASSIVE_4001);
            _conditional3001 = AssetDatabase.LoadAssetAtPath<PassiveAbility>(CONDITIONAL_3001);
        }

        [Test]
        public void ApplyPassive_Passive4001_ShouldGiveOneAbility()
        {
            var controller = Hero.AddComponent<PassivesController>();

            var passiveSpec = controller.ApplyPassive(_passive4001);

            Assert.NotNull(passiveSpec);
            Assert.AreEqual(1, AbilitySystem.GrantedAbilities.Count);
        }

        [Test]
        public void ApplyPassive_Passive4001WithPassiveType_ShouldActivateTwice()
        {
            var controller = Hero.AddComponent<PassivesController>();
            var passive = _passive4001;

            var first = controller.ApplyPassive(passive);
            var second = controller.ApplyPassive(passive);

            Assert.AreEqual(2, AbilitySystem.GrantedAbilities.Count);
            Assert.AreNotEqual(first.AbilitySO, passive);
            Assert.AreNotEqual(second.AbilitySO, passive);
            Assert.AreEqual(2, EffectSystem.AppliedEffects.Count);
        }

        [Test]
        public void RemovePassive_ClonePassive_CloneAbilitySOShouldBeDestroyed()
        {
            var controller = Hero.AddComponent<PassivesController>();
            var passive = _passive4001;
            var first = controller.ApplyPassive(passive);
            var second = controller.ApplyPassive(passive);

            controller.RemovePassive(second);

            Assert.IsNull(second.AbilitySO);
        }

        [Test]
        public void ApplyPassive_ConditionalPassive3001Twice_OnlyOneGranted()
        {
            var controller = Hero.AddComponent<PassivesController>();

            var first = controller.ApplyPassive(_conditional3001);
            var second = controller.ApplyPassive(_conditional3001);

            Assert.AreEqual(1, AbilitySystem.GrantedAbilities.Count);
            Assert.AreEqual(first.AbilitySO, _conditional3001);
            Assert.AreEqual(second.AbilitySO, _conditional3001);
            Assert.AreEqual(first, second);
        }
    }
}