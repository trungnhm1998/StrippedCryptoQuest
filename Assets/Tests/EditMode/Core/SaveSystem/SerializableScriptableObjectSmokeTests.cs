using System.Collections;
using Core.Runtime.SaveSystem.ScriptableObjects;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;

namespace Tests.EditMode.Core.SaveSystem
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