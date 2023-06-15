﻿using UnityEditor;
using UnityEngine;

namespace CryptoQuest.EditorTools.Attributes.ReadOnlyAttribute.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var previousGUIState = GUI.enabled;

            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = previousGUIState;
        }
    }
}