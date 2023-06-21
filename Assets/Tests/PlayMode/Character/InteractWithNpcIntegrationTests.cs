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
        [UnityTest]
        public IEnumerator Interact_WithNpcIntegrationScene_ShouldReturnCorrectDataFromSO()
        {
            var npcGameObject = new GameObject("NPC");
            var npcInstance = npcGameObject.AddComponent<NPC>();

            var dialog = ScriptableObject.CreateInstance<DialogsScriptableObject>();

            dialog.messages = new List<string>() { "Hello World", "Hello World 2" };

            npcInstance.SetDialog(dialog);

            var interactable = npcGameObject.GetComponent<IInteractable>();

            foreach (var message in dialog.messages)
            {
                var currentMessage = interactable.Interact();

                Assert.AreEqual(message, currentMessage);
            }

            yield return null;
        }
    }
}