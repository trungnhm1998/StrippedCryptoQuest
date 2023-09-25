using CryptoQuest.Quest.Authoring;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.System.QuestSystem
{
    [CustomEditor(typeof(AbstractObjective), true)]
    public class CutsceneTaskEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _uxml = default;
        private AbstractObjective _target => target as AbstractObjective;
        private Button _completeButton;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _uxml.CloneTree(root);

            _completeButton = root.Q<Button>("complete-btn");
            _completeButton.clicked += OnButtonCompleteClicked;

            return root;
        }

        private void OnButtonCompleteClicked()
        {
            _target.OnComplete();
        }
    }
}