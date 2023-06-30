using CryptoQuest.Character;
using IndiGames.Core.Events.ScriptableObjects.Dialogs;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor.NPC
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