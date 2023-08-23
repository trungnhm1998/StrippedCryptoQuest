using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace IndiGamesEditor.GameplayAbilitySystem
{
    [CustomEditor(typeof(AbilitySystemBehaviour))]
    public class AbilitySystemBehaviourEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _uxml;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _uxml.CloneTree(root);

            // bind granted-abilities label to the length
            var grantedAbilitiesLabel = root.Q<Label>("granted-abilities");
            var gas = (AbilitySystemBehaviour)target;
            // register event when granted abilities changed
            gas.AbilityGrantedEvent += _ => { grantedAbilitiesLabel.text = $"{gas.GrantedAbilities.Count}"; };

            return root;
        }
    }
}