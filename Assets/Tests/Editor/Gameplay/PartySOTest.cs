using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.PlayerParty;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Gameplay
{
    [TestFixture]
    public class PartySOTest
    {
        private PartySO _partySO;
        private IParty _party;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _partySO = AssetDatabase.LoadAssetAtPath<PartySO>(
                "Assets/ScriptableObjects/Battle/Characters/PartySO.asset");

            Assert.NotNull(_partySO, "Cannot find PartySO");
            Assert.NotNull(_partySO.Members, "Player team not found");
        }

        [SetUp]
        public void Setup()
        {
            _party = Object.Instantiate(_partySO);
        }

        [Test]
        public void Sort_LastToFirst_ShouldShiftCorrect()
        {
            var members = _party.Members;
            var orgMember = new List<CharacterSpec>(members);
            var result = _party.Sort(members.Length - 1, 0);
            Assert.IsTrue(result);
            Assert.AreEqual(members[0], orgMember[3]);
        }

        [Test]
        public void Sort_FirstToLast_ShouldShiftCorrect()
        {
            var members = _party.Members;
            var orgMember = new List<CharacterSpec>(members);
            var result = _party.Sort(0, members.Length - 1);
            Assert.IsTrue(result);
            Assert.AreEqual(members[3], orgMember[0]);
        }

        [Test]
        [TestCase(0, 2)]
        [TestCase(2, 1)]
        public void Sort_ShouldCorrect(int sourceIndex, int destinationIndex)
        {
            var members = _party.Members;
            var sourceMember = members[sourceIndex];
            var result = _party.Sort(sourceIndex, destinationIndex);
            Assert.IsTrue(result);
            Assert.AreEqual(sourceMember, members[destinationIndex]);
        }

        [Test]
        [TestCase(0, 4)]
        [TestCase(-1, 2)]
        public void Sort_ShouldFail(int sourceIndex, int destinationIndex)
        {
            Debug.unityLogger.logEnabled = false;
            var result = _party.Sort(sourceIndex, destinationIndex);
            Assert.IsFalse(result);
        }

        [Test]
        public void Sort_EmptyDestination_ShouldFail()
        {
            Debug.unityLogger.logEnabled = false;
            _party.Members[1] = new CharacterSpec();
            var result = _party.Sort(0, 1);
            Assert.IsFalse(result);
        }

        [Test]
        public void Sort_SameDestination_ShouldFail()
        {
            Debug.unityLogger.logEnabled = false;
            var result = _party.Sort(0, 0);
            Assert.IsFalse(result);
        }
    }
}