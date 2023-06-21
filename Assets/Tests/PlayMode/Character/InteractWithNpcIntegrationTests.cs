using System.Collections;
using System.Collections.Generic;
using Core.Runtime.Character;
using Core.Runtime.Events.ScriptableObjects.Dialogs;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Character
{
    public class InteractWithNpcIntegrationTests
    {
        private DialogsScriptableObject _dialogSO;
        private GameObject _npcGameObject;

        [SetUp]
        public void Setup()
        {
            _npcGameObject = new GameObject("NPC");

            _dialogSO = ScriptableObject.CreateInstance<DialogsScriptableObject>();
            _dialogSO.messages = new List<string>();
        }


        [UnityTest]
        public IEnumerator Interact_WithNpcIntegrationScene_ShouldReturnCorrectDataFromSO()
        {
            var mockMessage = new List<string>() { "Hello World", "Hello World 2" };
            _dialogSO.messages.AddRange(mockMessage);
            _npcGameObject.AddComponent<NPC>();

            DialogController dialogController = _npcGameObject.GetComponent<DialogController>();
            dialogController.SetDialog(_dialogSO);

            IInteractable interactable = _npcGameObject.GetComponent<NPC>();

            foreach (var message in _dialogSO.messages)
            {
                var currentMessage = interactable.Interact();

                Assert.AreEqual(message, currentMessage);
            }

            yield return null;
        }
    }
}