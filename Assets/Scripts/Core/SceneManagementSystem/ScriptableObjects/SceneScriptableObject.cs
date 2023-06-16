using Core.Common;
using Core.SaveSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.SceneManagementSystem.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scene Management/Scene Scriptable Object")]
    public class SceneScriptableObject : SerializableScriptableObject
    {
        public SceneAssetReference SceneReference;
    }
}