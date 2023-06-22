using System.Collections;
using System.Collections.Generic;
using Core.Runtime.Events.ScriptableObjects.Dialogs;
using CryptoQuest.Characters;
using CryptoQuest.Input;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Character
{
    public class InteractWithNpcIntegrationTests
    {
        private DialogsScriptableObject _dialogSO;
        private DialogController _dialogController;
        private Npc _npcGameObject;
        private GameObject _eventSystem;

        const string NPC_PREFAB_PATH = "Assets/Prefabs/Characters/NPCs/NPC.prefab";
        const string EVENT_SYSTEM_PATH = "Assets/Prefabs/UI/Shared/EventSystem.prefab";

        private List<string> DIALOG_TEST = new List<string>
        {
            "Hello World", "Hello World 2", "Hello World 3", "Hello World 4"
        };

        [SetUp]
        public void Setup()
        {
            _npcGameObject = AssetDatabase.LoadAssetAtPath(NPC_PREFAB_PATH, typeof(Npc)) as Npc;
            _npcGameObject = Object.Instantiate(_npcGameObject);

            _eventSystem = AssetDatabase.LoadAssetAtPath(EVENT_SYSTEM_PATH, typeof(GameObject)) as GameObject;
            _eventSystem = Object.Instantiate(_eventSystem);

            _dialogSO = ScriptableObject.CreateInstance<DialogsScriptableObject>();
            _dialogSO.Messages = new List<string>();
            _dialogSO.Messages.AddRange(DIALOG_TEST);

            _dialogController = _npcGameObject.GetComponent<DialogController>();
            _dialogController.SetDialogData(_dialogSO);
        }


        [UnityTest]
        public IEnumerator Interact_WithNpc_ShouldReturnCorrectDataFromSO()
        {
            int expectedCount = DIALOG_TEST.Count;
            bool hasCorrectData = true;

            for (int expected = 1; expected < expectedCount; expected++)
            {
                var nextIndex = _dialogController.GetNextIndex();
                if (expected != nextIndex)
                {
                    hasCorrectData = false;
                    break;
                }
            }


            Assert.IsTrue(hasCorrectData);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Interact_WithInputMediator_CanSendAndReceiveEvent()
        {
            var keyboard = InputSystem.AddDevice<Keyboard>();
            try
            {
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E));
                InputSystem.Update();

                Assert.That(keyboard.eKey.isPressed, Is.True);
            }
            finally
            {
                InputSystem.RemoveDevice(keyboard);
            }


            yield return null;
        }
    }
}