using Core.SceneManagementSystem.Events.ScriptableObjects;
using Core.SceneManagementSystem.ScriptableObjects;
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
            loadSceneEventChannelSO.LoadingRequested += sceneSO =>
            {
                hasCalled = sceneSO == mockScene;
            };

            loadSceneEventChannelSO.OnRaiseEvent(mockScene);

            Assert.IsTrue(hasCalled);
        }
    }
}