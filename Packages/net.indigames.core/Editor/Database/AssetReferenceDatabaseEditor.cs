using IndiGames.Core.Database;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine.UIElements;

namespace IndiGamesEditor.Core.Database
{
    [CustomEditor(typeof(AssetReferenceDatabaseT), true)]
    public class AssetReferenceDatabaseEditor : Editor
    {
        private AssetReferenceDatabaseT Target => (AssetReferenceDatabaseT)target;
        private SerializedProperty _plugins;
        private ReorderableList _pluginList;

        protected virtual void OnEnable()
        {
            _plugins = serializedObject.FindProperty("_plugins");
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            // add Load all data button
            root.Add(new Button(() =>
            {
                Target.Editor_FetchDataInProject();
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            })
            {
                text = "Load all data",
            });
            return root;
        }
    }
}