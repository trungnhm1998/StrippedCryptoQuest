﻿using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode.SceneManagementSystem
{
    public class StartupLoaderTests
    {
        [UnityTest]
        public IEnumerator ManagerScene_Loaded()
        {
            const string startupSceneName = "Startup";

            yield return SceneManager.LoadSceneAsync(startupSceneName, LoadSceneMode.Single);

            Assert.That(SceneManager.GetActiveScene().name == startupSceneName);

            var startupLoader = GameObject.Find("StartupLoader");

            Assert.NotNull(startupLoader);
            
            var managerScene = SceneManager.GetSceneByName("Manager");
            
            Assert.That(managerScene.isLoaded == false);
        }
    }
}