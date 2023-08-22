using System.Collections;
using CryptoQuest.Audio;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Linq;
using CryptoQuest.Gameplay.PlayerParty;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using System.Collections.Generic;

namespace CryptoQuest.Tests.Runtime.PlayerParty
{
    [TestFixture]
    [Category("Integration")]
    public class PartySOTest
    {
        private const string TEST_SCENE = "Assets/Tests/Runtime/PlayerParty/PartyTest.unity";

        private PartySO _partySO;
        private PartySortListener _partySortListener;

        public class PartySortListener 
        {
            public bool IsSuccess;
            private PartySO _party;

            public PartySortListener(PartySO partySO)
            {
                _party = partySO;
                partySO.SortCompleted += SortCompleted;
            }

            private void SortCompleted(bool isSuccess)
            {
                IsSuccess = isSuccess;
                _party.SortCompleted -= SortCompleted;
            }
        }

        [UnitySetUp]
        public IEnumerator OneTimeSetup()
        {
            var partySOGUID = AssetDatabase.FindAssets("t:PartySO").FirstOrDefault();
            _partySO = AssetDatabase.LoadAssetAtPath<PartySO>(AssetDatabase.GUIDToAssetPath(partySOGUID));

            Assert.NotNull(_partySO, "Cannot find PartySO");

            yield return EditorSceneManager.LoadSceneInPlayMode(TEST_SCENE,
                new LoadSceneParameters(LoadSceneMode.Single));

            Assert.NotNull(_partySO.PlayerTeam, "Player team not found");
            _partySortListener = new PartySortListener(_partySO);
        }

        [Test]
        public void Sort_LastToFirst_ShouldShiftCorrect()
        {
            var members = _partySO.PlayerTeam.Members;
            var orgMember = new List<AbilitySystemBehaviour>(members);
            _partySO.Sort(members.Count - 1, 0);
            Assert.AreEqual(members[0], orgMember[3]);
            Assert.AreEqual(members[1], orgMember[0]);
            Assert.AreEqual(members[2], orgMember[1]);
            Assert.AreEqual(members[3], orgMember[2]);
        }

        [Test]
        public void Sort_FirstToLast_ShouldShiftCorrect()
        {
            var members = _partySO.PlayerTeam.Members;
            var orgMember = new List<AbilitySystemBehaviour>(members);
            _partySO.Sort(0, members.Count - 1);
            Assert.AreEqual(members[3], orgMember[0]);
            Assert.AreEqual(members[2], orgMember[3]);
            Assert.AreEqual(members[1], orgMember[2]);
            Assert.AreEqual(members[0], orgMember[1]);
        }

        [Test]
        [TestCase(0, 2)]
        [TestCase(2, 1)]
        public void Sort_ShouldCorrect(int sourceIndex, int destinationIndex)
        {
            var members = _partySO.PlayerTeam.Members;
            var sourceMember = members[sourceIndex];
            _partySO.Sort(sourceIndex, destinationIndex);
            Assert.IsTrue(_partySortListener.IsSuccess);
            Assert.AreEqual(sourceMember, members[destinationIndex]);
        }

        [Test]
        [TestCase(0, 4)]
        [TestCase(-1, 2)]
        public void Sort_ShouldFail(int sourceIndex, int destinationIndex)
        {
            _partySO.Sort(sourceIndex, destinationIndex);
            LogAssert.Expect(LogType.Error, "Invalid source or destination index");
            Assert.IsFalse(_partySortListener.IsSuccess);
        }

        [Test]
        [TestCase(0)]
        public void Sort_SameDestination_ShouldSuccess(int sourceIndex)
        {
            _partySO.Sort(sourceIndex, sourceIndex);
            Assert.IsTrue(_partySortListener.IsSuccess);
        }
    }
}