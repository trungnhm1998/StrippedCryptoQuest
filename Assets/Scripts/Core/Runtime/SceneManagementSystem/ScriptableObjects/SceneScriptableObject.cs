using Core.Runtime.Common;
using Core.Runtime.SaveSystem.ScriptableObjects;
using UnityEngine;

namespace Core.Runtime.SceneManagementSystem.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scene Management/Scene Scriptable Object")]
    public class SceneScriptableObject : SerializableScriptableObject
    {
        public enum Type
        {
            Location = 0,
            Menu = 1,

            Boot = 2,
            GlobalManager = 3,
            GameplayManager = 4,

            WorkInProgress = 5,
        }

        public Type SceneType;
        public SceneAssetReference SceneReference;
    }
}