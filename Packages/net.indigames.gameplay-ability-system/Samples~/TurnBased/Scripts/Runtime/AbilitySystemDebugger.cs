using System;
using System.Collections.Generic;
using UnityEngine;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;

namespace IndiGames.GameplayAbilitySystem.Sample
{
    public class AbilitySystemDebugger : MonoBehaviour
    {
    #if UNITY_EDITOR || DEVELOPMENT_BUILD 
        [SerializeField] private AbilitySystemBehaviour _owner;
        [SerializeField] private BattleUnitBase _battleUnit;
        [SerializeField] private AbilitySystemBehaviour _target;
        [SerializeField] private bool _showDebug = false;
        [SerializeField] private Rect _windowRect = new Rect(20, 20, 120, 50);
        [SerializeField] private float _windowWidth = 400;
        [SerializeField] private int _windowId;
        [Header("Abilities to add")]
        [SerializeField] private List<AbilityScriptableObject> _availableSkills = new();
        [SerializeField] private List<AbilityScriptableObject> _defaultGrantSkills = new();
        [Header("Effects to add")]
        [SerializeField] private List<EffectScriptableObject> _effects = new();
        [Header("Modifier to add")]
        [SerializeField] private AttributeModifier[] _modifiers;

        private AttributeSystemBehaviour _attributeSystem;
        private bool _showAttributes = true;
        private bool _showSkills = true;
        private bool _showTargetUnit = false;
        private bool _showAppliedEffects = true;
        private bool _showApplyEffects = false;
        private bool _showApplyModifiers = false;
        private Rect _minimizeRect;
        
        private void OnValidate()
        {
            if (_owner == null)
            {
                _owner = GetComponent<AbilitySystemBehaviour>();
            }
            if (_battleUnit == null)
            {
                _battleUnit = GetComponent<BattleUnitBase>();
            }
        }

        private void Start()
        {
            _minimizeRect = _windowRect;
            _attributeSystem = _owner.AttributeSystem;
            GrantDefaulSkills();
        }

        private void GrantDefaulSkills()
        {
            foreach (var skill in _defaultGrantSkills)
            {
                _owner.GiveAbility(skill);
            }
        }

        private void OnGUI()
        {
            if (_owner == null) return;
            // DragWindow
            Rect newRect;
            var inputRect = _showDebug ? _windowRect : _minimizeRect;

            GUILayout.BeginVertical();
            newRect = GUILayout.Window(_windowId, inputRect, DoMyWindow, $"{_owner.name} Stats", GUILayout.ExpandHeight(true));
            GUILayout.EndVertical();

            if (!_showDebug)
                _windowRect.position = newRect.position;
            else
                _windowRect = newRect;
        }

        void DoMyWindow(int windowID)
        {
            // Insert a huge dragging area at the end.
            // This gets clipped to the window (like all other controls) so you can never
            // drag the window from outside it.
            GUI.DragWindow(new Rect(0, 0, 200, 20));
            _showDebug = GUILayout.Toggle(_showDebug, "Render Debugs");
            if (_showDebug)
                RenderDebug();
        }

        private void RenderDebug()
        {
            GUILayout.BeginHorizontal("box", GUILayout.Width(_windowWidth));
            {
                GUILayout.BeginVertical();
                RenderAttributesDebug();
                RenderAppliedEffect();
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal("box", GUILayout.Width(_windowWidth));
            {
                GUILayout.BeginVertical();
                RenderListSkillsDebug();
                RenderGrantedSkillsDebug();
                RenderListTargetsDebug();
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal("box", GUILayout.Width(_windowWidth));
            {
                GUILayout.BeginVertical();
                RenderAddEffect();
                RenderAddModifiers();
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void RenderAddModifiers()
        {
            _showApplyModifiers = GUILayout.Toggle(_showApplyModifiers, "Show add modifiers");
            if (!_showApplyModifiers) return;

            foreach (var modifier in _modifiers)
            {
                if (GUILayout.Button($"Add modifier {modifier.Attribute.name}"))
                {
                    if (_attributeSystem.AddModifierToAttribute(modifier.Modifier, modifier.Attribute, out _))
                    {
                        Debug.Log($"Modifier added to {modifier.Attribute.name}");
                    }
                }
            }
        }

        private void RenderAddEffect()
        {
            _showApplyEffects = GUILayout.Toggle(_showApplyEffects, "Show add effects");
            if (!_showApplyEffects) return;
            if (GUILayout.Button("Remove all skills"))
            {
                _owner.RemoveAllAbilities();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Turns: ");
            var txtLevel = GUILayout.TextField("1");
            GUILayout.Label("Base Power: ");
            var txtLevelRate = GUILayout.TextField("0");
            GUILayout.EndHorizontal();
            foreach (var effect in _effects)
            {
                if (GUILayout.Button($"Add effect {effect.name}"))
                {
                    var effectSpec = _owner.EffectSystem.GetEffect(effect, name, new SkillParameters());
                    _target.EffectSystem.ApplyEffectToSelf(effectSpec);
                }
            }
        }

        private void RenderAppliedEffect()
        {
            _showAppliedEffects = GUILayout.Toggle(_showAppliedEffects, "Show applied effects/modfiers");
            if (!_showAppliedEffects) return;
            foreach (var appliedEffect in _owner.EffectSystem.AppliedEffects)
            {
                var abstractEffect = appliedEffect.EffectSpec;
                var isActive = abstractEffect.IsExpired ? "Inactive" : "Active";
                string optional = $"({isActive})";
                float computedValue = 0;
                if (abstractEffect.EffectSO.EffectDetails.Modifiers[0].ModifierComputationMethod)
                    computedValue = abstractEffect.EffectSO.EffectDetails.Modifiers[0].ModifierComputationMethod
                        .CalculateMagnitude(abstractEffect);
                string details = "";
                if (abstractEffect.Parameters != null)
                {
                    var skillParams = (SkillParameters) abstractEffect.Parameters;
                    details = $"\nTurns: {skillParams.continuesTurn} | Base Power: {skillParams.basePower}";
                }

                if (abstractEffect is DurationalEffect durationalEffect)
                {
                    var remainingDuration = durationalEffect.RemainingDuration;
                    optional = $"({remainingDuration:0.00})";
                }

                GUILayout.Label(
                    $"Effect: {abstractEffect.Origin}.{abstractEffect.EffectSO.name} {optional} {details}");
            }
        }

        private void RenderAttributesDebug()
        {
            _showAttributes = GUILayout.Toggle(_showAttributes, "Show Attributes");
            if (!_showAttributes) return;
            foreach (var attribute in _attributeSystem.AttributeValues)
            {
                GUILayout.Label($"{attribute.Attribute.name}: {attribute.CurrentValue}\n"
                    + $"(equipments [+{attribute.CoreModifier.Additive}] [*{attribute.CoreModifier.Multiplicative}]) " 
                        + $"(skills/effects [+{attribute.Modifier.Additive}] [*{attribute.Modifier.Multiplicative}])");
            }
        }
        
        private void RenderListTargetsDebug()
        {
            if (!_showTargetUnit || _battleUnit.Targets == null) return;
            GUILayout.Label("Select a target");
            foreach (var target in _battleUnit.Targets)
            {
                if (target == null) continue;
                GUI.enabled = !_battleUnit.TargetContainer.Targets.Contains(target);
                var buttonLabel = (GUI.enabled ? "Select" : "Selected") + $" target {target.name}";
                if (GUILayout.Button(buttonLabel))
                {
                    _battleUnit.SelectSingleTarget(target);
                    _showTargetUnit = false;
                }
                GUI.enabled = true;
            }
        }

        private void RenderListSkillsDebug()
        {
            _showSkills = GUILayout.Toggle(_showSkills, "Show Available Abilities");
            if (!_showSkills) return;
            foreach (var skill in _availableSkills)
            {
                if (GUILayout.Button($"Grant skill {skill.name}"))
                {
                    var effectSpec = _owner.GiveAbility(skill);
                }
            }
        }
        
        private void RenderGrantedSkillsDebug()
        {
            if (_battleUnit.SelectedSkill != null) return;
            GUILayout.Label("Granted Skills");
            foreach (var skill in _owner.GrantedAbilities.Abilities)
            {
                if (GUILayout.Button($"Select skill {skill.AbilitySO.name}"))
                {
                    _battleUnit.SelectSkill(skill);
                    _showTargetUnit = true;
                }
            }
        }
    #endif
    }
}