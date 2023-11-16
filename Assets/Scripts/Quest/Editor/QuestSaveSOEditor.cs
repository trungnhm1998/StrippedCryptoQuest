using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CryptoQuest.Quest.Editor
{
    [CustomEditor(typeof(QuestSaveSO))]
    public class QuestSaveSOEditor : UnityEditor.Editor
    {
        private QuestSaveSO Target => target as QuestSaveSO;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            // draw clear button
            var clearButton = new Button(() =>
            {
                Target.InProgressQuest.Clear();
                Target.CompletedQuests.Clear();
            });
            clearButton.text = "Clear";
            root.Add(clearButton);

            return root;
        }
    }
}