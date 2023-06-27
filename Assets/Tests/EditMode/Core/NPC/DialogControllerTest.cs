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
        private DialogueScriptableObject _dialogueSo;
        private NpcDialogController _npcDialogController;
        private GameObject _npcGameObject;


        [SetUp]
        public void Setup()
        {
            _npcGameObject = new GameObject();
            _npcGameObject.AddComponent<NpcDialogController>();
            _npcGameObject.AddComponent<NpcDialogController>();

            _dialogueSo = ScriptableObject.CreateInstance<DialogueScriptableObject>();
        }
    }
}