using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Beast;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Tests.Editor.Beast.Builder;
using IndiGames.Core.Common;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Beast
{
    [TestFixture]
    public class BeastPassiveControllerTests
    {
        private PartyManager _partyManager;
        private BeastPassiveApplier _beastApplier;

        [SetUp]
        public void Setup()
        {
            var party = AssetDatabase.LoadAssetAtPath<PartySO>("Assets/ScriptableObjects/Party/HeroesParty.asset");
            var mockParty = AssetDatabase.LoadAssetAtPath<PartyManager>("Assets/Prefabs/Gameplay/PartyManager.prefab");
            _partyManager = GameObject.Instantiate(mockParty);

            ServiceProvider.Provide<IPartyController>(_partyManager);
            var behaviour = Substitute.For<IBeastEquippingBehaviour>();
            _beastApplier = new BeastPassiveApplier(behaviour, _partyManager);

            var heroes = _partyManager.GetComponentsInChildren<HeroBehaviour>();
            foreach (var hero in heroes)
            {
                hero.Spec = party.GetParty()[0].Hero;
            }
        }

        [Test]
        public void ApplyPassive_AllHeroesInvalid_ShouldNotApplySkill()
        {
            var heroes = _partyManager.GetComponentsInChildren<HeroBehaviour>();
            foreach (var hero in heroes)
            {
                hero.Spec = new();
            }

            var passive =
                AssetDatabase.LoadAssetAtPath<PassiveAbility>(
                    "Assets/ScriptableObjects/Character/Skills/Passive/4001.asset");
            IBeast beast = A.Beast.WithPassive(passive).Build();
            _beastApplier.ApplyPassive(beast);

            var abilitySystems = _partyManager.GetComponentsInChildren<AbilitySystemBehaviour>();
            foreach (var system in abilitySystems)
            {
                Assert.AreEqual(0, system.GrantedAbilities.Count);
            }
        }

        [Test]
        public void ApplyPassive_TwoOutOfFourInvalid_ShouldNotApplySkill()
        {
            var heroes = _partyManager.GetComponentsInChildren<HeroBehaviour>();
            for (int i = 0; i < 4; i++)
            {
                if (i < 2) continue;
                heroes[i].Spec = new();
            }

            var passive =
                AssetDatabase.LoadAssetAtPath<PassiveAbility>(
                    "Assets/ScriptableObjects/Character/Skills/Passive/4001.asset");
            IBeast beast = A.Beast.WithPassive(passive).Build();
            _beastApplier.ApplyPassive(beast);

            var abilitySystems = _partyManager.GetComponentsInChildren<AbilitySystemBehaviour>();
            var j = 0;
            foreach (var system in abilitySystems)
            {
                if (j < 2)
                    Assert.AreEqual(1, system.GrantedAbilities.Count);
                else
                    Assert.AreEqual(0, system.GrantedAbilities.Count);
                j++;
            }
        }

        [Test]
        public void ApplyPassive_WithNewBeastHaveNullPassive_OldPassiveShouldBeRemoved()
        {
            var originalPassive = ScriptableObject.CreateInstance<PassiveAbility>();
            IBeast beast = A.Beast.WithPassive(originalPassive).Build();
            _beastApplier.ApplyPassive(beast);

            var abilitySystems = _partyManager.GetComponentsInChildren<AbilitySystemBehaviour>();

            beast = A.Beast.WithPassive(null).Build();
            _beastApplier.ApplyPassive(beast);
            
            foreach (var system in abilitySystems)
            {
                var oldPassiveToRemove = system.GrantedAbilities.FirstOrDefault(a => a.AbilitySO == originalPassive);
                Assert.IsNull(oldPassiveToRemove);
            }
        }
        
        [Test]
        public void ApplyPassive_WithNewNullBeast_OldPassiveShouldBeRemoved()
        {
            var originalPassive = ScriptableObject.CreateInstance<PassiveAbility>();
            IBeast beast = A.Beast.WithPassive(originalPassive).Build();
            _beastApplier.ApplyPassive(beast);

            var abilitySystems = _partyManager.GetComponentsInChildren<AbilitySystemBehaviour>();

            _beastApplier.ApplyPassive(null);
            
            foreach (var system in abilitySystems)
            {
                var oldPassiveToRemove = system.GrantedAbilities.FirstOrDefault(a => a.AbilitySO == originalPassive);
                Assert.IsNull(oldPassiveToRemove);
            }
        }
        
        [Test]
        public void ApplyPassive_WithNewNullBeastObject_OldPassiveShouldBeRemoved()
        {
            var originalPassive = ScriptableObject.CreateInstance<PassiveAbility>();
            IBeast beast = A.Beast.WithPassive(originalPassive).Build();
            _beastApplier.ApplyPassive(beast);

            var abilitySystems = _partyManager.GetComponentsInChildren<AbilitySystemBehaviour>();

            _beastApplier.ApplyPassive(NullBeast.Instance);
            
            foreach (var system in abilitySystems)
            {
                var oldPassiveToRemove = system.GrantedAbilities.FirstOrDefault(a => a.AbilitySO == originalPassive);
                Assert.IsNull(oldPassiveToRemove);
            }
        }

        [Test]
        public void ApplyPassive_BeastWithPassive4001_AllMemberInPartyShouldHavePassive()
        {
            var passive =
                AssetDatabase.LoadAssetAtPath<PassiveAbility>(
                    "Assets/ScriptableObjects/Character/Skills/Passive/4001.asset");
            IBeast beast = A.Beast.WithPassive(passive).Build();
            _beastApplier.ApplyPassive(beast);

            var abilitySystems = _partyManager.GetComponentsInChildren<AbilitySystemBehaviour>();
            foreach (var system in abilitySystems)
            {
                var grantedPassive = system.GrantedAbilities.FirstOrDefault(a => a.AbilitySO == beast.Passive);
                Assert.AreEqual(passive, grantedPassive.AbilitySO);
            }
        }

        [Test]
        public void ApplyPassive_BeastWithNullPassive_AllMemberInPartyWillNotApplyBeastPassive()
        {
            IBeast beast = A.Beast.WithPassive(null).Build();
            _beastApplier.ApplyPassive(beast);

            var abilitySystems = _partyManager.GetComponentsInChildren<AbilitySystemBehaviour>();
            foreach (var system in abilitySystems)
            {
                var grantedPassive = system.GrantedAbilities.FirstOrDefault(a => a.AbilitySO == beast.Passive);
                Assert.IsNull(grantedPassive);
            }
        }
        
        [Test]
        public void
            ApplyPassive_WithNewBeastHavePassive_OldPassiveShouldBeChangedToNewPassive()
        {
            var passive4001 =
                AssetDatabase.LoadAssetAtPath<PassiveAbility>(
                    "Assets/ScriptableObjects/Character/Skills/Passive/4001.asset");
            var passive4002 =
                AssetDatabase.LoadAssetAtPath<PassiveAbility>(
                    "Assets/ScriptableObjects/Character/Skills/Passive/4002.asset");

            IBeast beast = A.Beast.WithPassive(passive4001).Build();
            _beastApplier.ApplyPassive(beast);

            var abilitySystems = _partyManager.GetComponentsInChildren<AbilitySystemBehaviour>();

            foreach (var system in abilitySystems)
            {
                var effectSystem = system.GetComponent<EffectSystemBehaviour>();
                effectSystem.AttributeSystem = system.GetComponent<AttributeSystemBehaviour>();
                var grantedPassive = system.GrantedAbilities.FirstOrDefault(a => a.AbilitySO == beast.Passive);
                Assert.AreEqual(passive4001, grantedPassive.AbilitySO);
            }

            var oldPassive = beast.Passive;
            beast = A.Beast.WithPassive(passive4002).Build();
            var newPassive = beast.Passive;

            _beastApplier.ApplyPassive(beast);
            foreach (var system in abilitySystems)
            {
                var oldPassiveToRemove = system.GrantedAbilities.FirstOrDefault(a => a.AbilitySO == oldPassive);
                var getNewPassive = system.GrantedAbilities.FirstOrDefault(a => a.AbilitySO == newPassive);
                Assert.IsNull(oldPassiveToRemove);
                Assert.AreEqual(passive4002, getNewPassive.AbilitySO);
            }
        }

        [Test]
        public void ApplyPassive_WithNullBeast_ShouldReturn()
        {
            Assert.DoesNotThrow(() => _beastApplier.ApplyPassive(null));
        }
    }
}