using System.Collections.Generic;
using CryptoQuest.Gameplay.PlayerParty;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Gameplay
{
    [TestFixture]
    public class PartyManagerTests
    {
        // private PartySO _partySO;
        // private IPartyController _party;
        //
        // [SetUp]
        // public void Setup()
        // {
        //     var gameObject = new GameObject();
        //     _party = gameObject.AddComponent<PartyManager>();
        // }
        //
        // [Test]
        // public void Sort_LastToFirst_ShouldShiftCorrect()
        // {
        //     var members = _party.Slots;
        //     var orgMember = new List<CharacterSpec>(members);
        //     var result = _party.Sort(members.Length - 1, 0);
        //     Assert.IsTrue(result);
        //     Assert.AreEqual(members[0], orgMember[3]);
        // }
        //
        // [Test]
        // public void Sort_FirstToLast_ShouldShiftCorrect()
        // {
        //     var members = _party.Slots;
        //     var orgMember = new List<CharacterSpec>(members);
        //     var result = _party.Sort(0, members.Length - 1);
        //     Assert.IsTrue(result);
        //     Assert.AreEqual(members[3], orgMember[0]);
        // }
        //
        // [Test]
        // [TestCase(0, 2)]
        // [TestCase(2, 1)]
        // public void Sort_ShouldCorrect(int sourceIndex, int destinationIndex)
        // {
        //     var members = _party.Slots;
        //     var sourceMember = members[sourceIndex];
        //     var result = _party.Sort(sourceIndex, destinationIndex);
        //     Assert.IsTrue(result);
        //     Assert.AreEqual(sourceMember, members[destinationIndex]);
        // }
        //
        // [Test]
        // [TestCase(0, 4)]
        // [TestCase(-1, 2)]
        // public void Sort_ShouldFail(int sourceIndex, int destinationIndex)
        // {
        //     Debug.unityLogger.logEnabled = false;
        //     var result = _party.Sort(sourceIndex, destinationIndex);
        //     Assert.IsFalse(result);
        // }
        //
        // [Test]
        // public void Sort_EmptyDestination_ShouldFail()
        // {
        //     Debug.unityLogger.logEnabled = false;
        //     _party.Slots[1] = new CharacterSpec();
        //     var result = _party.Sort(0, 1);
        //     Assert.IsFalse(result);
        // }
        //
        // [Test]
        // public void Sort_SameDestination_ShouldFail()
        // {
        //     Debug.unityLogger.logEnabled = false;
        //     var result = _party.Sort(0, 0);
        //     Assert.IsFalse(result);
        // }
    }
}