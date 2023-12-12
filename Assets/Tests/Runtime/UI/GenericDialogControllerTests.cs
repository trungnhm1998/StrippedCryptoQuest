using System.Collections;
using CryptoQuest.Input;
using CryptoQuest.UI.Dialogs.BattleDialog;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CryptoQuest.Tests.Runtime.UI
{
#if UNITY_EDITOR
    [TestFixture]
    public class GenericDialogControllerTests
    {
        private GenericDialogController _controller;
        private UIGenericDialog _dialog;

        [UnitySetUp]
        public virtual IEnumerator Setup()
        {
            _controller = new GameObject().AddComponent<GenericDialogController>();
            var so = new SerializedObject(_controller);
            var guid = AssetDatabase.GUIDFromAssetPath("Assets/Prefabs/UI/Dialog/GenericDialog.prefab");
            var assetRef = new AssetReference(guid.ToString());
            so.FindProperty("_prefab").boxedValue = assetRef;
            so.ApplyModifiedProperties();
            yield return null;
        }

        [UnityTest]
        public IEnumerator CoInstantiate_ShouldReleaseAfterOneSecond()
        {
            yield return _controller.CoInstantiate((prefab) => prefab.Hide(), true, 1f);
            yield return new WaitForSeconds(1f);
            yield return null;

            Assert.AreEqual(0, _controller.transform.childCount);
            Assert.IsFalse(_controller.Handler.IsValid());
        }

        [UnityTest]
        public IEnumerator CoInstantiate_ShouldNotReleaseWhileStillUsing()
        {
            void InstantiatedCallback(UIGenericDialog prefab)
            {
                _dialog = prefab;
                _dialog.Show();
            }

            yield return _controller.CoInstantiate(InstantiatedCallback, true, 1f);
            yield return new WaitForSeconds(1f);
            yield return null;
            Assert.AreEqual(1, _controller.transform.childCount);
            _dialog.Hide();
            yield return new WaitForSeconds(1f);
            yield return null;

            Assert.AreEqual(0, _controller.transform.childCount);
        }
    }

    [TestFixture]
    public class UIGenericDialogTest : InputTestFixture
    {
        private GenericDialogController _controller;
        private UIGenericDialog _dialog;

        [UnitySetUp]
        public virtual IEnumerator Setup()
        {
            _controller = new GameObject().AddComponent<GenericDialogController>();
            var so = new SerializedObject(_controller);
            var guid = AssetDatabase.GUIDFromAssetPath("Assets/Prefabs/UI/Dialog/GenericDialog.prefab");
            var assetRef = new AssetReference(guid.ToString());
            so.FindProperty("_prefab").boxedValue = assetRef;
            so.ApplyModifiedProperties();
            yield return null;
        }

        [UnityTest]
        public IEnumerator NextDialoguePressed_ShouldHide()
        {
            var keyboard = InputSystem.AddDevice<Keyboard>();
            var input = AssetDatabase.LoadAssetAtPath<InputMediatorSO>(
                "Assets/ScriptableObjects/Input/InputMediatorSO.asset");

            void InstantiatedCallback(UIGenericDialog prefab)
            {
                _dialog = prefab;
                prefab
                    .RequireInput()
                    .Show();
            }

            yield return _controller.CoInstantiate(InstantiatedCallback, true, 1f);
            // simulate press enter using the new input system
            Press(keyboard.enterKey);
            yield return null;
            Assert.IsFalse(_dialog.Content.activeSelf);

            yield return new WaitForSeconds(1f);
            yield return null;

            Assert.AreEqual(0, _controller.transform.childCount);
            Assert.IsFalse(_controller.Handler.IsValid());
        }
    }
#endif
}