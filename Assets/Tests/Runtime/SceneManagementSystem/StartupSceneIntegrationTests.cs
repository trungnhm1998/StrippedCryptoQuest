using System.Collections;
using IndiGames.Core.SceneManagementSystem;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.Runtime.SceneManagementSystem
{
    public class StartupSceneIntegrationTests
    {
        private const int FRAMES_TO_WAIT = 360;

        [UnityTest]
        public IEnumerator ManagerScene_Loaded()
        {
            const string startupSceneName = "Startup";

            yield return SceneManager.LoadSceneAsync(startupSceneName, LoadSceneMode.Single);

            Assert.That(SceneManager.GetActiveScene().name == startupSceneName);

            var startupLoader = GameObject.Find("StartupLoader");

            Assert.NotNull(startupLoader);

            var startupLoaderComponent = startupLoader.GetComponent<StartupLoader>();

            Assert.NotNull(startupLoaderComponent);

            var framesToWait = FRAMES_TO_WAIT;
            while (framesToWait >= 0)
            {
                framesToWait--;
                yield return null;
            }

            Assert.That(SceneManager.GetSceneByName("GlobalManagers").isLoaded);

            var sceneLoaderGo = GameObject.Find("SceneLoader");

            Assert.NotNull(sceneLoaderGo);

            var sceneLoader = sceneLoaderGo.GetComponent<LinearGameSceneLoader>();

            Assert.NotNull(sceneLoader);

            framesToWait = FRAMES_TO_WAIT;
            while (framesToWait >= 0)
            {
                framesToWait--;
                yield return null;
            }

            Assert.That(SceneManager.GetSceneByName(startupSceneName).isLoaded == false);

            Assert.That(startupLoader == null);
            framesToWait = FRAMES_TO_WAIT;
            while (framesToWait >= 0)
            {
                framesToWait--;
                yield return null;
            }

            Assert.That(SceneManager.GetSceneByName("Title").isLoaded);
        }
    }
}