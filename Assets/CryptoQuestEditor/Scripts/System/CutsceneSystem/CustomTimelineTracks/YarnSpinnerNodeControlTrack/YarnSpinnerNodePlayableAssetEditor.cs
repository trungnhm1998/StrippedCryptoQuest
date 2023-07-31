using System.Collections.Generic;
using System.Linq;
using CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

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
                provider.YarnProject = TimelineEditor.inspectedDirector.GetGenericBinding(TimelineEditor.selectedClip.GetParentTrack()) as YarnProject;
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

    public class YarnNodeEntriesProvider : ScriptableObject, ISearchWindowProvider
    {
        public UnityAction<string> EntrySelected;
        public YarnProject YarnProject;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var searchTree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Yarn Nodes")),
            };

            List<string> sortedNodeNames = YarnProject.NodeNames.ToList();
            sortedNodeNames.Sort();

            for (var index = 0; index < sortedNodeNames.Count; index++)
            {
                var node = sortedNodeNames[index];
                Debug.Log(node);
                searchTree.Add(new SearchTreeEntry(new GUIContent(node)) { level = 1, userData = node });
            }

            return searchTree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            if (YarnProject == null) return false;
            if (SearchTreeEntry.userData == null) return false;

            var node = SearchTreeEntry.userData as string;

            EntrySelected.Invoke(node);
            return true;
        }
    }
}