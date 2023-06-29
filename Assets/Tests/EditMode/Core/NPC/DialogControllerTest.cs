using System.Collections.Generic;
using Core.Runtime.Events.ScriptableObjects.Dialogs;
using CryptoQuest.Character;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests.EditMode.Core.NPC
{
    [TestFixture]
    public class DialogControllerTest
    {
        private DialogueScriptableObject _dialogueSo;
        private DialogManager _dialogManager;
        private GameObject _npcGameObject;


        [SetUp]
        public void Setup()
        {
            _npcGameObject = new GameObject();
            _npcGameObject.AddComponent<DialogManager>();
            _npcGameObject.AddComponent<DialogManager>();

            _dialogueSo = ScriptableObject.CreateInstance<DialogueScriptableObject>();
        }
    }
}