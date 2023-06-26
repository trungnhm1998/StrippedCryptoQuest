using System.Collections;
using System.Collections.Generic;
using Core.Runtime.Events.ScriptableObjects;
using CryptoQuest.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CryptoQuest.UI
{
    public class UIDialogPrefabIntergrationTests
    {
        private const string FILTER = "UIDialog t:prefab";
        private IDialog _dialog;

        [SetUp]
        public void Setup()
        {
            var uiDialogPrefabGUIDs = UnityEditor.AssetDatabase.FindAssets(FILTER);

            var guid = uiDialogPrefabGUIDs[0];

            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);

            var dialogPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<UIDialog>(path);

            _dialog = GameObject.Instantiate(dialogPrefab);

        }

        [UnityTest]
        public IEnumerator GiveUIDialog_WhenShowEventRaised_ThenContentShouldShown()
        {
            var eventGUIDs = UnityEditor.AssetDatabase.FindAssets("TurnOnSpeechDialogEvent");

            var guid = eventGUIDs[0];

            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);

            var showEvent = UnityEditor.AssetDatabase.LoadAssetAtPath<VoidEventChannelSO>(path);

            var content = GameObject.Find("UIDialog(Clone)").transform.GetChild(0);

            showEvent.RaiseEvent();

            Assert.IsTrue(_dialog.IsShown);

            yield break;
        }
    }
}