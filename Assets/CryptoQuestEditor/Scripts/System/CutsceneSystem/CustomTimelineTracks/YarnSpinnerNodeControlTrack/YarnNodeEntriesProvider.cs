using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace CryptoQuestEditor.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack
{
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