using CryptoQuest.Core.Common;
using CryptoQuest.Core.SaveSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Core.SceneManagementSystem.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scene Management/Scene Scriptable Object")]
    public class SceneScriptableObject : SerializableScriptableObject
    {
        public SceneAssetReference sceneAssetReference;
    }
}