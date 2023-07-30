using CryptoQuest.Character.Emote;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.Character.Emote
{
    [CustomEditor(typeof(EmoteSO))]
    public class EmoteSOEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _visualTreeAsset.CloneTree(root);

            var previewImage = root.Q<Image>("preview-image");
            previewImage.image = ((EmoteSO)target).ReactionIcon.texture;

            return root;
        }
    }
}