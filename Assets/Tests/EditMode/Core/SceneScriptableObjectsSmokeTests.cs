using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Core
{
    [TestFixture]
    public class SceneScriptableObjectsSmokeTests
    {
        [Test]
        public void SceneScriptableObjects_CreatedCorrectly()
        {
            var guids = UnityEditor.AssetDatabase.FindAssets("t:SceneScriptableObject");

            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var sceneScriptableObject = UnityEditor.AssetDatabase.LoadAssetAtPath<SceneScriptableObject>(path);
                Assert.IsNotEmpty(sceneScriptableObject.SceneReference.AssetGUID,
                    $"{path} has no scene asset reference.");
            }
        }
    }
}