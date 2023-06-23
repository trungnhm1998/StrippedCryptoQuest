using Core.Runtime.Events.ScriptableObjects;
using CryptoQuest.UI;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.CryptoQuest.UI
{
    [TestFixture]
    public class UIDialogTest
    {
        private GameObject _dialogGameObject;
        private GameObject _contentGameObject;
        private IDialog _dialog;

        [SetUp]
        public void Setup()
        {
            _dialogGameObject = new GameObject();
            _contentGameObject = new GameObject();
            _contentGameObject.SetActive(false);
            _dialog = _dialogGameObject.AddComponent<UIDialog>();
            _dialog.Content = _contentGameObject;
        }

        [Test]
        public void Show_NotCalled_ContentShouldBeHidden()
        {
            Assert.IsFalse(_dialog.IsShown);
        }

        [Test]
        public void Show_Called_DialogUIShouldShown()
        {
            _dialog.Show();

            Assert.IsTrue(_dialog.IsShown);
        }

        [Test]
        public void ShowDialogEvent_Raised_DialogUIShouldShown()
        {
            var showDialogEvent = ScriptableObject.CreateInstance<VoidEventChannelSO>();

            _dialog.ShowDialogEvent = showDialogEvent;

            IEvents e = _dialogGameObject.GetComponent<IEvents>();

            e.RegisterEvents();

            showDialogEvent.RaiseEvent();

            e.UnregisterEvents();
        }
    }
}