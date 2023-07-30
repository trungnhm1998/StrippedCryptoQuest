using CryptoQuest.Character.Reaction;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.Character.Emote
{
    [CustomEditor(typeof(Reaction))]
    public class EmoteSOEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _visualTreeAsset.CloneTree(root);

            var previewImage = root.Q<Image>("preview-image");
            previewImage.image = ((Reaction)target).ReactionIcon.texture;

            return root;
        }
    }
}