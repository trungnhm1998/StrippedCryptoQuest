using CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack;
using CryptoQuest.System.Dialogue.Events;
using CryptoQuest.System.Dialogue.YarnManager;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Timeline;
using UnityEngine;

namespace CryptoQuestEditor.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack
{
    [CustomEditor(typeof(YarnSpinnerNodePlayableAsset)), CanEditMultipleObjects]
    public class YarnSpinnerNodePlayableAssetEditor : Editor
    {
        private YarnSpinnerNodePlayableAsset Target => target as YarnSpinnerNodePlayableAsset;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (target == null) return;

            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Yarn Node Name", GUILayout.ExpandWidth(false), GUILayout.Width(250));

            if (GUILayout.Button(Target.YarnNodeName, EditorStyles.popup))
            {
                var provider = CreateInstance<YarnNodeEntriesProvider>(); // TODO: cache this with yarn project
                var yarnConfig =
                    TimelineEditor.inspectedDirector.GetGenericBinding(TimelineEditor.selectedClip.GetParentTrack()) as
                        YarnProjectConfigSO;
                provider.YarnProject = yarnConfig?.YarnProject;
                provider.EntrySelected = nodeName =>
                {
                    Debug.Log($"Selected {nodeName}");
                    Target.YarnNodeName = nodeName;
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

            serializedObject.ApplyModifiedProperties();
        }
    }
}