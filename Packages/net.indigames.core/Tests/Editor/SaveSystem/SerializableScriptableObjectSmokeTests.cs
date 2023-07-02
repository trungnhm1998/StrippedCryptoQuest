using IndiGames.Core.SaveSystem.ScriptableObjects;
using NUnit.Framework;
using UnityEditor;

namespace IndiGames.Core.EditorTests.SaveSystem
{
    public class SerializableScriptableObjectSmokeTests
    {
        [Test]
        public void SerializableScriptableObjects_CreatedCorrectly()
        {
            var guids = AssetDatabase.FindAssets("t:SerializableScriptableObject");

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var serializableScriptableObject = AssetDatabase.LoadAssetAtPath<SerializableScriptableObject>(path);
                Assert.IsNotEmpty(serializableScriptableObject.Guid, $"{path} has no asset reference.");
            }
        }
    }
}