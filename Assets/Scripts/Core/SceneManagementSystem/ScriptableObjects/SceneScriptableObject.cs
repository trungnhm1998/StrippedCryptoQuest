using Core.Common;
using Core.SaveSystem.ScriptableObjects;
using UnityEngine;

namespace Core.SceneManagementSystem.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scene Management/Scene Scriptable Object")]
    public class SceneScriptableObject : SerializableScriptableObject
    {
        public SceneAssetReference sceneAssetReference;
    }
}