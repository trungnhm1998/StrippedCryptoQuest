using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Core
{
    [TestFixture]
    public class LoadSceneEventChannelSOTests
    {
        [Test]
        public void OnRaiseEvent_RaisesEvent()
        {
            var hasCalled = false;
            var mockScene = ScriptableObject.CreateInstance<SceneScriptableObject>();
            var loadSceneEventChannelSO = ScriptableObject.CreateInstance<LoadSceneEventChannelSO>();
            loadSceneEventChannelSO.LoadingRequested += sceneSO => { hasCalled = sceneSO == mockScene; };

            loadSceneEventChannelSO.OnRaiseEvent(mockScene);

            Assert.IsTrue(hasCalled);
        }

        [Test]
        public void OnRaiseEvent_WithNoSubscriber_ShouldLogWarning()
        {
            var mockScene = ScriptableObject.CreateInstance<SceneScriptableObject>();
            var loadSceneEventChannelSO = ScriptableObject.CreateInstance<LoadSceneEventChannelSO>();

            var hasCalled = loadSceneEventChannelSO.OnRaiseEvent(mockScene);

            Assert.IsFalse(hasCalled);
        }

        [Test]
        public void OnRaiseEvent_WithNull_ShouldReturnFalse()
        {
            var loadSceneEventChannelSO = ScriptableObject.CreateInstance<LoadSceneEventChannelSO>();
            loadSceneEventChannelSO.LoadingRequested += sceneSO => { };

            var hasCalled = loadSceneEventChannelSO.OnRaiseEvent(null);

            Assert.IsFalse(hasCalled);
        }
    }
}