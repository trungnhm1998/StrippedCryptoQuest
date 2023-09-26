using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEditor;
using CryptoQuest.Character;
using UnityEditor.VersionControl;
using CryptoQuest.Shop.UI.ScriptableObjects;

namespace CryptoQuest.Tests.Runtime.Shop
{
    [TestFixture]
    [Category("Integration")]
    public class ShopTest
    {
#if UNITY_EDITOR
        private const string SHOP_SCENE = "Assets/Tests/Runtime/Shop/Shop.unity";
        private string shopObjectName = "ShopPanel";
        private string shopDialogName = "DialogPanel";
        private ShowShopEventChannelSO _showShopEvent;

        [UnitySetUp]
        public IEnumerator OneTimeSetUp()
        {
            //Load event

            var eventGuids = AssetDatabase.FindAssets("t:ShowShopEventChannelSO");

            foreach (var eventGuid in eventGuids)
            {
                _showShopEvent = AssetDatabase.LoadAssetAtPath<ShowShopEventChannelSO>(AssetDatabase.GUIDToAssetPath(eventGuid));
            }

            Assert.IsNotNull(_showShopEvent,  "show shop event could not be null!");

            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(SHOP_SCENE, new LoadSceneParameters(LoadSceneMode.Single));


        }
        [UnityTest]
        public IEnumerator ShowShop_WithShowShopEventSO_ShouldShowShopUIAndWelcomeDialog()
        {
            Assert.IsNotNull(_showShopEvent, "Event was null");
            _showShopEvent.RaiseEvent(null);


            yield return null;

            var GO = GameObject.Find(shopObjectName);


            Assert.IsNotNull(GO, "Shop doesn't show after trigger event");

            Assert.IsTrue(GO.gameObject.activeSelf, "Expected : shop panel should be displayed");

            var dialogGO = GameObject.Find(shopObjectName);


            Assert.IsNotNull(dialogGO, "Message Dialog doesn't show after trigger event");

            Assert.IsTrue(dialogGO.gameObject.activeSelf, "Expected: Dialog message should be displayed");

        }



#endif
    }
}
