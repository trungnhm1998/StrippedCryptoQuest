using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Core.Runtime.SceneManagementSystem.ScriptableObjects
{
    [CustomEditor(typeof(SceneScriptableObject), true)]
    public class SceneSOEditor : UnityEditor.Editor
    {
        public SceneScriptableObject sceneToLoad;
        private void OnEnable()
        {
            sceneToLoad = (SceneScriptableObject)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Load Scene"))
            {
                LoadSceneAdditive(sceneToLoad); 
            }
        }
        public void LoadSceneAdditive(SceneScriptableObject sceneSO)
        {
            sceneSO.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        }
    }
}


