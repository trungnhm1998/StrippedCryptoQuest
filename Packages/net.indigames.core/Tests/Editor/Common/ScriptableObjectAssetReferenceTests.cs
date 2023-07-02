using IndiGames.Core.Common;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace IndiGames.Core.EditorTests.Common
{
    public class ScriptableObjectAssetReferenceTests
    {
        [Test]
        public void ValidateAsset_WithAssetPath_ReturnsTrue()
        {
            var mockSO = ScriptableObject.CreateInstance<MockSO>();

            // create Temp folder if it doesn't exist
            if (!AssetDatabase.IsValidFolder("Assets/Temp"))
            {
                AssetDatabase.CreateFolder("Assets", "Temp");
            }

            var path = AssetDatabase.GenerateUniqueAssetPath("Assets/Temp/MockSO.asset");
            AssetDatabase.CreateAsset(mockSO, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var guid = AssetDatabase.AssetPathToGUID(path);
            var assetReference = new ScriptableObjectAssetReference<MockSO>(guid);

            Assert.IsTrue(assetReference.ValidateAsset(path));

            AssetDatabase.DeleteAsset("Assets/Temp");
        }

        [Test]
        public void ValidateAssets_WithObject_ShouldReturnTrue()
        {
            var scriptableObjectAssetReference = new ScriptableObjectAssetReference<MockSO>("0");

            var mockSO = ScriptableObject.CreateInstance<MockSO>();

            Assert.True(scriptableObjectAssetReference.ValidateAsset(mockSO));
        }

        [Test]
        public void ValidateAssets_WithEmptyPath_ShouldReturnFalse()
        {
            var scriptableObjectAssetReference = new ScriptableObjectAssetReference<MockSO>("0");

            Assert.False(scriptableObjectAssetReference.ValidateAsset(string.Empty));
        }

        [Test]
        public void ValidateAssets_WithGameObject_ShouldReturnFalse()
        {
            var scriptableObjectAssetReference = new ScriptableObjectAssetReference<MockSO>("0");

            var gameObject = new GameObject();

            Assert.False(scriptableObjectAssetReference.ValidateAsset(gameObject));
        }
    }
}