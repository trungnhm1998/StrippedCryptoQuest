using CryptoQuest.Quest.Authoring;
using CryptoQuestEditor.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CryptoQuestEditor.System.YarnSpinner
{
    [CustomEditor(typeof(YarnDialogWithQuestSo)), CanEditMultipleObjects]
    public class YarnDialogWithQuestSoEditor : Editor
    {
        private YarnDialogWithQuestSo Target => target as YarnDialogWithQuestSo;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (target == null) return;

            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Yarn Node Name", GUILayout.ExpandWidth(false), GUILayout.Width(250));

            if (GUILayout.Button(Target.YarnQuestDef.YarnNode, EditorStyles.popup))
            {
                var provider = CreateInstance<YarnNodeEntriesProvider>(); // TODO: cache this with yarn project
                var yarnConfig = Target.YarnQuestDef.YarnProjectConfig;
                provider.YarnProject = yarnConfig?.YarnProject;
                provider.EntrySelected = nodeName =>
                {
                    Debug.Log($"Selected {nodeName}");
                    Target.YarnQuestDef.YarnNode = nodeName;
                    serializedObject.ApplyModifiedProperties();
                    // save the asset
                    EditorUtility.SetDirty(target);
                    AssetDatabase.SaveAssets();
                };
                SearchWindow.Open(
                    new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                    provider);
            }

            GUILayout.Button("Refresh"); // this is a hack to get the YarnNodeName to update

            EditorGUILayout.EndHorizontal();
        }
    }
}