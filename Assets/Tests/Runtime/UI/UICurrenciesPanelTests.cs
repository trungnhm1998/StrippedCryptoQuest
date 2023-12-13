using System.Collections;
using CryptoQuest.Events.UI;
using CryptoQuest.UI.Menu;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.UI
{
    [TestFixture, Category("Integration")]
    public class UICurrenciesPanelTests
    {
        public const string EVENT_PATH = "Assets/ScriptableObjects/Events/UI/Menu/ShowWalletEventChannelSO.asset";
        public const float WAIT_TIME = 1f;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            LogAssert.ignoreFailingMessages = true;
            _showEvent = AssetDatabase.LoadAssetAtPath<ShowWalletEventChannelSO>(EVENT_PATH);
        }

        private bool _setup;
        private ShowWalletEventChannelSO _showEvent;
        private UICurrenciesPanel _panel;
        private Transform _gold;
        private Transform _diamond;
        private Transform _soul;
        private Transform _content;

        [UnitySetUp]
        public IEnumerator UnityOnetimeSetup()
        {
            if (_setup) yield break;
            _setup = true;
            var scene = "Assets/Scenes/WIP/Playground.unity";
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(scene,
                new LoadSceneParameters(LoadSceneMode.Single));
            yield return new WaitForSeconds(2);
            _panel = Object.FindObjectOfType<UICurrenciesPanel>();
            _content = _panel.transform.Find("Content");
            _gold = _panel.transform.Find("Content/Background/Gold");
            _diamond = _panel.transform.Find("Content/Background/Diamonds");
            _soul = _panel.transform.Find("Content/Background/Souls");
        }

        [UnityTest]
        public IEnumerator Show_AllCurrenciesShouldBeShown()
        {
            _showEvent.Show();
            yield return new WaitForSeconds(WAIT_TIME);

            Assert.IsTrue(_panel.gameObject.activeSelf);
            Assert.IsTrue(_gold.gameObject.activeSelf);
            Assert.IsTrue(_diamond.gameObject.activeSelf);
            Assert.IsTrue(_soul.gameObject.activeSelf);
        }

        [UnityTest]
        public IEnumerator Show_DisableAll_ShouldNotShowsAnyCurrencyUI()
        {
            _showEvent.EnableAll(false).Show();
            yield return new WaitForSeconds(WAIT_TIME);
            
            Assert.IsFalse(_gold.gameObject.activeSelf);
            Assert.IsFalse(_diamond.gameObject.activeSelf);
            Assert.IsFalse(_soul.gameObject.activeSelf);
        }

        [UnityTest]
        public IEnumerator Show_WithGoldAndDiamond_ShouldHideSouls()
        {
            _showEvent.EnableAll().EnableSouls(false).Show();
            yield return new WaitForSeconds(WAIT_TIME);

            Assert.IsTrue(_panel.gameObject.activeSelf);
            Assert.IsTrue(_gold.gameObject.activeSelf);
            Assert.IsTrue(_diamond.gameObject.activeSelf);
            Assert.IsFalse(_soul.gameObject.activeSelf);
        }

        [UnityTest]
        public IEnumerator Show_OnlyGold_ShouldHideOthers()
        {
            _showEvent.EnableAll(false).EnableGold().Show();
            yield return new WaitForSeconds(WAIT_TIME);

            Assert.IsTrue(_panel.gameObject.activeSelf);
            Assert.IsTrue(_gold.gameObject.activeSelf);
            Assert.IsFalse(_diamond.gameObject.activeSelf);
            Assert.IsFalse(_soul.gameObject.activeSelf);
        }

        [UnityTest]
        public IEnumerator Show_OnlyDiamonds_ShouldHideOthers()
        {
            _showEvent.EnableAll().EnableGold(false).EnableSouls(false).Show();
            yield return new WaitForSeconds(WAIT_TIME);
            
            Assert.IsTrue(_panel.gameObject.activeSelf);
            Assert.IsFalse(_gold.gameObject.activeSelf);
            Assert.IsTrue(_diamond.gameObject.activeSelf);
            Assert.IsFalse(_soul.gameObject.activeSelf);
        }

        [UnityTest]
        public IEnumerator Hide_ShouldHide()
        {
            _showEvent.EnableAll().Show();
            yield return new WaitForSeconds(WAIT_TIME);
            _showEvent.Hide();
            yield return new WaitForSeconds(WAIT_TIME);

            Assert.IsFalse(_content.gameObject.activeSelf);
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            _showEvent.Hide();
            yield return new WaitForSeconds(WAIT_TIME);
        }
    }
}