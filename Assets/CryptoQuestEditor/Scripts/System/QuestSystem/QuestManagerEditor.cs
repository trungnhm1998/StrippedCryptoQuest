using System;
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

        private ObjectField _questField;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _uxml.CloneTree(root);

            var clearButton = root.Q<Button>("clear-button");
            clearButton.clicked += ClearData;

            _questField = root.Q<ObjectField>("quest-field");
            var addQuestInfoButton = root.Q<Button>("add-quest-info");

            addQuestInfoButton.clicked += AddQuest;

            return root;
        }

        private void AddQuest()
        {
            if (_questField == null) return;

            QuestSO questData = _questField.value as QuestSO;

            if (questData == null) return;

            QuestInfo currentQuestInfo = questData.CreateQuest(Target);

            Target.InProgressQuest ??= new();
            Target.InProgressQuest.Add(currentQuestInfo);
        }

        private void ClearData()
        {
            Target.InProgressQuest = new();
            Target.CompletedQuests = new();
        }
    }
}