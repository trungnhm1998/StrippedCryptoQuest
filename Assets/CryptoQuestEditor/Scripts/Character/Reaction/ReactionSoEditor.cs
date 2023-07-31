using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.Character.Reaction
{
    [CustomEditor(typeof(CryptoQuest.Character.Reaction.Reaction))]
    public class ReactionSoEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _visualTreeAsset.CloneTree(root);

            var previewImage = root.Q<Image>("preview-image");
            previewImage.image = ((CryptoQuest.Character.Reaction.Reaction)target).ReactionIcon.texture;

            return root;
        }
    }
}