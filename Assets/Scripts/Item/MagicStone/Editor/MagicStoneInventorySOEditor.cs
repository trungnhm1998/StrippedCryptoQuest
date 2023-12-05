using CryptoQuest.Item.MagicStone;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.Item.MagicStone
{
    [CustomEditor(typeof(MagicStoneInventory))]
    public class MagicStoneInventorySOEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            
            
            return root;
        }
    }
}