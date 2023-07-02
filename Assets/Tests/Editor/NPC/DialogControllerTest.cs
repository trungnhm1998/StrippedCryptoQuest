using CryptoQuest.Character;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.NPC
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