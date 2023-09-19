using System;
using System.Collections.Generic;
using System.Linq;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace IndiGamesEditor.GameplayAbilitySystem
{
    [CustomPropertyDrawer(typeof(ReferenceEnumAttribute))]
    public class ReferenceEnumAttributeDrawer : PropertyDrawer
    {
        private IEnumerable<Type> _types;

        private IEnumerable<Type> GetTypes(Type type)
        {
            if (_types == null)
            {
                _types = AppDomain.CurrentDomain.GetAllImplementedTypes(type);
            }

            return _types;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            try
            {
                var propertyPath = property.propertyPath;
                var selectedType = property.managedReferenceValue.GetType();
                var typeDefs = property.managedReferenceFieldTypename.Split(' ');
                var asm = AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == typeDefs[0]);
                var typeName = typeDefs[1];
                // get type from full type name using reflection
                var genericType = asm.GetType(typeName); // got the interface
                var types = GetTypes(genericType).ToList();
                var selected = EditorGUILayout.Popup("Derived Types",
                    types.FindIndex(t => t.Name == selectedType.Name),
                    types.Select(t => t.Name).ToArray());
                bool changed = selected != -1 && selectedType != types[selected];
                if (changed)
                {
                    var serializedObject = property.serializedObject;
                    serializedObject.Update();
                    var referenceProperty = serializedObject.FindProperty(propertyPath);
                    referenceProperty.managedReferenceValue = Activator.CreateInstance(types[selected]);
                    serializedObject.ApplyModifiedProperties();

                    var obj = Activator.CreateInstance(types[selected]);
                    property.managedReferenceValue = obj;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
            catch (Exception e)
            {
                // This attribute only support reference type
            }

            EditorGUILayout.PropertyField(property, label, true);
        }
    }
}