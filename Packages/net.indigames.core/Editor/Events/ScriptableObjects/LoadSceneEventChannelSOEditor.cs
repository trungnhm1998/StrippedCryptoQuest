using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace IndiGamesEditor.Core.Events.ScriptableObjects
{
    [CustomEditor(typeof(LoadSceneEventChannelSO))]
    public class LoadSceneEventChannelSOEditor : Editor
    {
        private SceneScriptableObject _sceneScriptableObject;
        private LoadSceneEventChannelSO Target => target as LoadSceneEventChannelSO;
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            
            // draw object field with SceneScriptableObject type
            var sceneScriptableObjectField = new ObjectField("Scene Scriptable Object");
            sceneScriptableObjectField.objectType = typeof(SceneScriptableObject);
            sceneScriptableObjectField.RegisterValueChangedCallback(evt =>
            {
                _sceneScriptableObject = evt.newValue as SceneScriptableObject;
            });
            
            root.Add(sceneScriptableObjectField);
            
            // draw RaiseEvent button
            var raiseEventButton = new Button(() =>
            {
                Target.RequestLoad(_sceneScriptableObject);
            });
            raiseEventButton.text = "Raise Event";
            root.Add(raiseEventButton);

            return root;
        }
    }
}