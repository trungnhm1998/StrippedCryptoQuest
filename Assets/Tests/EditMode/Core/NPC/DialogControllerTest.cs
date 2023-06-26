using System.Collections.Generic;
using Core.Runtime.Events.ScriptableObjects.Dialogs;
using CryptoQuest.Characters;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests.EditMode.Core.NPC
{
    [TestFixture]
    public class DialogControllerTest
    {
        private DialogsScriptableObject _dialogSO;
        private NpcDialogController _npcDialogController;
        private GameObject _npcGameObject;


        [SetUp]
        public void Setup()
        {
            _npcGameObject = new GameObject(); 
            _npcGameObject.AddComponent<NpcDialogController>(); 
            _npcGameObject.AddComponent<NpcDialogController>();
            
            _dialogSO = ScriptableObject.CreateInstance<DialogsScriptableObject>();

        }

        [Test]
        public void GetNextIndex_WithEmptyDialogSO_ShouldReturnZero()
        {
            var index = _npcDialogController.GetNextIndex();

            Assert.AreEqual(0, index);
        }

        [Test]
        public void GetNextIndex_WithDummyDataDialogSO_ShouldReturnCorrectIndex()
        {
            // int expectedCount = DIALOG_TEST.Count;
            // bool hasCorrectData = true;
            //
            // for (int expected = 1; expected < expectedCount; expected++)
            // {
            //     var nextIndex = _npcDialogController.GetNextIndex();
            //     if (expected != nextIndex)
            //     {
            //         hasCorrectData = false;
            //         break;
            //     }
            // }
            //
            // Assert.IsTrue(hasCorrectData);
        }

        [Test]
        public void GetNextIndex_Called_ShouldReturnOne()
        {
            var index = _npcDialogController.GetNextIndex();
            var expected = 1;

            Assert.AreEqual(expected, index);
        }

        [Test]
        public void GetCurrentDialogIndex_ShouldReturnZeroByDefault()
        {
            var currentIndex = _npcDialogController.GetCurrentDialogIndex();
            var expectedIndex = 0;

            Assert.AreEqual(expectedIndex, currentIndex);
        }
    }
}