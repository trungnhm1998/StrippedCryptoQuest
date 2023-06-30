using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using NUnit.Framework;

namespace IndiGamesEditor.Core.Tests
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