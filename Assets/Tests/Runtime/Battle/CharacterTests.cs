using CryptoQuest.Battle;
using CryptoQuest.Battle.Components;
using CryptoQuest.Tests.Runtime.Battle.Builder;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Runtime.Battle
{
    public class CharacterTests : BattleFixtureBase
    {
        private GameObject _characterGameObject;
        private ICharacter _character;

        [SetUp]
        public void Setup()
        {
            _characterGameObject = CreateCharacterFromPrefab();
            _character = A.Character.WithPrefab(_characterGameObject).Build();
        }

        [Test, Category("Smokes")]
        public void Create_ShouldHaveAbilitySystem()
        {
            Assert.NotNull(_character.AbilitySystem);
        }

        [Test, Category("Smokes")]
        public void Create_ShouldHaveInterfaceStatsInitializer()
        {
            Assert.IsTrue(_characterGameObject.TryGetComponent<IStatsInitializer>(out _));
        }
    }
}