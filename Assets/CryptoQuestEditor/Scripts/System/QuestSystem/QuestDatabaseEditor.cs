using System.Collections.Generic;
using CryptoQuest.Quest.Authoring;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.System.QuestSystem
{
    [CustomEditor(typeof(QuestDatabase))]
    public class QuestDatabaseEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _uxml = default;

        private QuestDatabase _target => target as QuestDatabase;
        private Button _inCompleteAllButton;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _uxml.CloneTree(root);

            _inCompleteAllButton = root.Q<Button>("in-complete");

            ShowProgress(root);

            _inCompleteAllButton.clicked += OnButtonInCompletedClicked;

            return root;
        }

        private void ShowProgress(VisualElement root)
        {
            var progressCount = 0;
            foreach (var objective in _target.Quests)
            {
                var quest = objective as Quest;
                if (quest == null) continue;
                
                foreach (var task in quest.Tasks)
                {
                    if (task.IsCompleted)
                    {
                        progressCount++;
                    }
                }


                var progress = new ProgressBar();
                progress.lowValue = 0;
                progress.highValue = quest.Tasks.Length;
                progress.value = progressCount;
                progress.title = quest.name;
                progress.name = quest.name;

                root.Add(progress);
            }
        }

        private void OnButtonInCompletedClicked()
        {
            foreach (var objective in _target.Quests)
            {
                objective.Editor_SetCompleted(false);

                var quest = objective as Quest;
                foreach (var task in quest.Tasks)
                {
                    task.Editor_SetCompleted(false);
                }
            }
        }
    }
}