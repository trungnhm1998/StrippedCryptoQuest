using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.System.QuestSystem
{
    [CustomEditor(typeof(QuestManager))]
    public class QuestManagerEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _uxml;
        private QuestManager Target => target as QuestManager;

        private VisualElement _root;

        public override VisualElement CreateInspectorGUI()
        {
            _root = new VisualElement();
            InspectorElement.FillDefaultInspector(_root, serializedObject, this);
            _uxml.CloneTree(_root);

            InitializeListView("inprogress-quest", Target.SaveData?.InProgressQuest);
            InitializeListView("complete-quest", Target.SaveData?.CompletedQuests);

            return _root;
        }

        private void InitializeListView(string listViewName, List<string> targetCompletedQuests)
        {
            ListView listView = _root.Q<ListView>(listViewName);
            listView.makeItem = () => new Label();
            listView.bindItem = (element, index) =>
            {
                var label = element as Label;
                label.text = targetCompletedQuests.ElementAt(index);
            };
            listView.itemsSource = targetCompletedQuests;
            listView.fixedItemHeight = 20;
            listView.selectionType = SelectionType.None;
        }
    }
}