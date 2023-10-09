using System;
using System.Collections.Generic;
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

        private ListView _inprogressScrollView;
        private ListView _completeScrollView;


        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _uxml.CloneTree(root);

            var clearButton = root.Q<Button>("clear-button");
            clearButton.clicked += ClearData;

            _inprogressScrollView = root.Q<ListView>("inprogess-quest");
            _completeScrollView = root.Q<ListView>("complete-quest");
            ReloadData();

            return root;
        }


        private void ReloadData()
        {
            _inprogressScrollView.makeItem = () => new Label();
            Target.InProgressQuest.ForEach(quest =>
            {
                _inprogressScrollView.bindItem = (element, index) =>
                {
                    var label = element as Label;
                    label.text = quest.BaseData.name;
                };
            });
            _inprogressScrollView.itemsSource = Target.InProgressQuest;
            _inprogressScrollView.fixedItemHeight = 20;
            _inprogressScrollView.selectionType = SelectionType.None;

            _completeScrollView.makeItem = () => new Label();
            Target.CompletedQuests.ForEach(quest =>
            {
                _completeScrollView.bindItem = (element, index) =>
                {
                    var label = element as Label;
                    label.text = quest.BaseData.name;
                };
            });

            _completeScrollView.itemsSource = Target.CompletedQuests;
            _completeScrollView.fixedItemHeight = 20;
            _completeScrollView.selectionType = SelectionType.None;
        }

        private void ClearData() { }
    }
}