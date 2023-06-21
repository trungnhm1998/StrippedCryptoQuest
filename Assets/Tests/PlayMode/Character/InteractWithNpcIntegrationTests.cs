using System.Collections;
using System.Collections.Generic;
using Core.Runtime.Events.ScriptableObjects.Dialogs;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Character
{
    public class InteractWithNpcIntegrationTests
    {
        private DialogsScriptableObject _dialogSO;
        private NPC _npcGameObject;

        const string NPC_PREFAB_PATH = "Assets/Prefabs/Characters/NPCs/NPC.prefab";

        [SetUp]
        public void Setup()
        {
            _npcGameObject = AssetDatabase.LoadAssetAtPath(NPC_PREFAB_PATH, typeof(NPC)) as NPC;
            _npcGameObject = Object.Instantiate(_npcGameObject);

            _dialogSO = ScriptableObject.CreateInstance<DialogsScriptableObject>();
            _dialogSO.Messages = new List<string>() { "Hello World", "Hello World 2" };
        }


        [UnityTest]
        public IEnumerator Interact_WithNpc_ShouldReturnCorrectDataFromSO()
        {
            DialogController dialogController = _npcGameObject.GetComponent<DialogController>();
            dialogController.SetDialog(_dialogSO);

            foreach (var message in _dialogSO.Messages)
            {
                _npcGameObject.Interact();
                Assert.AreEqual(message, _npcGameObject.DialogData);
            }

            yield return null;
        }
    }
}