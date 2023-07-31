using CryptoQuest.Character;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.Character.Reaction
{
    [CustomEditor(typeof(ReactionBehaviour))]
    public class ReactionControllerEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;

        private ReactionBehaviour Target => (ReactionBehaviour)target;

        private CryptoQuest.Character.Reaction _reactionDef;
        private Button _showReactionButton;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _visualTreeAsset.CloneTree(root);

            var objectField = root.Q<ObjectField>("reaction-object-field");
            objectField.objectType = typeof(CryptoQuest.Character.Reaction);
            objectField.RegisterValueChangedCallback(evt => { _reactionDef = (CryptoQuest.Character.Reaction)evt.newValue; });

            _showReactionButton = root.Q<Button>("show-reaction-button");
            _showReactionButton.SetEnabled(Application.isPlaying);
            _showReactionButton.clicked += ShowReaction;

            return root;
        }

        private void ShowReaction()
        {
            if (_reactionDef != null) Target.ShowReaction(_reactionDef);
        }

        private void OnDisable()
        {
            if (_showReactionButton != null) _showReactionButton.clicked -= ShowReaction;
        }
    }
}