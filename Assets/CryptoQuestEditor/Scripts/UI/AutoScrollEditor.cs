using CryptoQuest.UI.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CryptoQuestEditor
{
    [CustomEditor(typeof(AutoScroll))]
    class AutoScrollEditor : Editor
    {
        public VisualTreeAsset Uxml;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            Uxml.CloneTree(root);
            InspectorElement.FillDefaultInspector(root, serializedObject, this);


            return root;
        }
    }
}