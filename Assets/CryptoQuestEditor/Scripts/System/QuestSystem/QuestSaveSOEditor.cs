using CryptoQuest.Quest;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.System.QuestSystem
{
    [CustomEditor(typeof(QuestSaveSO))]
    public class QuestSaveSOEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;
        private QuestSaveSO Target => target as QuestSaveSO;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _visualTreeAsset.CloneTree(root);

            var clearSaveButton = root.Q<Button>("clear-save-button");
            clearSaveButton.clicked += ClearSave;

            return root;
        }

        private void ClearSave()
        {
            Target.InProgressQuest.Clear();
            Target.CompletedQuests.Clear();

            EditorUtility.SetDirty(Target);
            AssetDatabase.SaveAssets();
        }
    }
}