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

        private List<string> DIALOG_TEST = new List<string>
        {
            "Hello World", "Hello World 2", "Hello World 3", "Hello World 4"
        };

        [SetUp]
        public void Setup()
        {
            _npcGameObject = new GameObject(); 
            _npcGameObject.AddComponent<NpcDialogController>(); 
            _npcGameObject.AddComponent<NpcDialogController>();
            
            _dialogSO = ScriptableObject.CreateInstance<DialogsScriptableObject>();
            _dialogSO.Messages = new List<string>();
            _dialogSO.Messages.AddRange(DIALOG_TEST);

            _npcDialogController = _npcGameObject.GetComponent<NpcDialogController>();
            _npcDialogController.SetDialogData(_dialogSO);
        }

        [Test]
        public void GetNextIndex_WithEmptyDialogSO_ShouldReturnZero()
        {
            _dialogSO.Messages.Clear();
            var index = _npcDialogController.GetNextIndex();

            Assert.AreEqual(0, index);
        }

        [Test]
        public void GetNextIndex_WithDummyDataDialogSO_ShouldReturnCorrectIndex()
        {
            int expectedCount = DIALOG_TEST.Count;
            bool hasCorrectData = true;

            for (int expected = 1; expected < expectedCount; expected++)
            {
                var nextIndex = _npcDialogController.GetNextIndex();
                if (expected != nextIndex)
                {
                    hasCorrectData = false;
                    break;
                }
            }

            Assert.IsTrue(hasCorrectData);
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