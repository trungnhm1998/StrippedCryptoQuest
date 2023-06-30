using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using NUnit.Framework;
using UnityEngine;

namespace IndiGamesEditor.Core.Tests
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

            loadSceneEventChannelSO.RequestLoad(mockScene);

            Assert.IsTrue(hasCalled);
        }

        [Test]
        public void OnRaiseEvent_WithNoSubscriber_ShouldLogWarning()
        {
            var hasCalled = false;
            var mockScene = ScriptableObject.CreateInstance<SceneScriptableObject>();
            var loadSceneEventChannelSO = ScriptableObject.CreateInstance<LoadSceneEventChannelSO>();

            loadSceneEventChannelSO.RequestLoad(mockScene);
            loadSceneEventChannelSO.LoadingRequested += sceneSO => { hasCalled = true; };

            Assert.IsFalse(hasCalled);
        }

        [Test]
        public void OnRaiseEvent_WithNull_ShouldNotCalled()
        {
            var hasCalled = false;
            var loadSceneEventChannelSO = ScriptableObject.CreateInstance<LoadSceneEventChannelSO>();
            loadSceneEventChannelSO.LoadingRequested += sceneSO => { hasCalled = true; };

            loadSceneEventChannelSO.RequestLoad(null);

            Assert.IsFalse(hasCalled);
        }
    }
}