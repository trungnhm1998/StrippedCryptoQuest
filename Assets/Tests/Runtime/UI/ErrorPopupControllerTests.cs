using System.Collections;
using CryptoQuest.Input;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using CryptoQuest.UI.Popups;
using CryptoQuest.Events;
using CryptoQuest.UI.Utilities;
using UnityEngine.InputSystem.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CryptoQuest.Tests.Runtime.UI
{
#if UNITY_EDITOR
    [TestFixture]
    public class ErrorPopupControllerTests : InputTestFixture
    {
        private ErrorPopupController _controller;
        private UIPopup _dialog;
        private float _testAutoRelease = 0.5f;
        private string _testString = "Test";
        private string _testLocalizeText = "TestLocalize";
        private StringEventChannelSO _showStringErrorEvent;
        private LocalizedStringEventChannelSO _showLocalizedErrorEvent;
        private Keyboard _keyboard;

        private const string EVENT_SO_PATH = "Assets/ScriptableObjects/Events/Popup/ShowErrorPopupEvent.asset";
        private const string LOCALIZE_EVENT_SO_PATH = "Assets/ScriptableObjects/Events/Popup/ShowLocalizedErrorPopupEvent.asset";
        private const string ERROR_POPUP_PATH = "Assets/Prefabs/UI/Popup/ErrorPopupController.prefab";

        [UnitySetUp]
        public override void Setup()
        {
            base.Setup();
            var controllerPrefab = AssetDatabase.LoadAssetAtPath<ErrorPopupController>(ERROR_POPUP_PATH);
            _controller = Object.Instantiate<ErrorPopupController>(controllerPrefab);
            var so = new SerializedObject(_controller);
            so.FindProperty("_autoReleaseTimeout").boxedValue = _testAutoRelease;
            so.ApplyModifiedProperties();

            var popupInputManager = _controller.GetComponent<PopupInputManager>();
            var inputSystem = _controller.GetOrAddComponent<InputSystemUIInputModule>();
            var inputSo = new SerializedObject(popupInputManager);
            inputSo.FindProperty("_inputSystemModule").boxedValue = inputSystem;
            inputSo.ApplyModifiedProperties();

            _showStringErrorEvent = AssetDatabase.LoadAssetAtPath<StringEventChannelSO>(EVENT_SO_PATH);
            _showLocalizedErrorEvent = AssetDatabase.LoadAssetAtPath<LocalizedStringEventChannelSO>(LOCALIZE_EVENT_SO_PATH);
            
            
            _keyboard = InputSystem.AddDevice<Keyboard>();
            var input = AssetDatabase.LoadAssetAtPath<InputMediatorSO>(
                "Assets/ScriptableObjects/Input/InputMediatorSO.asset");
        }

        [UnityTest]
        [TestCase(1, ExpectedResult = null)]
        [TestCase(3, ExpectedResult = null)]
        public IEnumerator ShowPopup_ShouldShowUp_CorrectNumberPopups(int eventCall)
        {
            for (int i = 0; i < eventCall; i++)
            {
                _showStringErrorEvent.RaiseEvent(_testString);
                yield return new WaitForSeconds(.5f);
            }
            Assert.AreEqual(eventCall + 1, _controller.transform.childCount);
            yield return null;
        }

        [UnityTest]
        [TestCase(1, ExpectedResult = null)]
        [TestCase(3, ExpectedResult = null)]
        public IEnumerator HideLastPopup_ShouldHideAfterPress_CorrectTimes(int eventCall)
        {
            for (int i = 0; i < eventCall; i++)
            {
                _showStringErrorEvent.RaiseEvent(_testString);
                yield return new WaitForSeconds(.5f);
            }

            for (int i = 0; i < eventCall; i++)
            {
                PressAndRelease(_keyboard.spaceKey);
                yield return new WaitForSeconds(.5f);
            }

            Assert.IsTrue(_controller.IsPopupsEmpty);
        }

        [UnityTest]
        public IEnumerator ReleaseAssets_ShouldReleaseAfterHide()
        {
            _showStringErrorEvent.RaiseEvent(_testString);

            yield return new WaitForSeconds(0.5f);

            PressAndRelease(_keyboard.spaceKey);
            yield return new WaitForSeconds(_testAutoRelease);
            Assert.AreEqual(1, _controller.transform.childCount);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _controller.gameObject.SetActive(false);
        }
    }
#endif
}