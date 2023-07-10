using System.Collections.Generic;
using UnityEngine;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;

namespace CryptoQuest.Gameplay.Battle
{
    public class StatsDebugger : MonoBehaviour
    {
    #if UNITY_EDITOR || DEVELOPMENT_BUILD 
        [SerializeField] private AbilitySystemBehaviour _owner;
        [SerializeField] private bool _showDebug = false;
        [SerializeField] private Rect _windowRect = new Rect(20, 20, 120, 50);
        [SerializeField] private float _windowWidth = 400;
        [SerializeField] private int _windowId;

        private AttributeSystemBehaviour _attributeSystem;
        private bool _showAttributes = true;
        private bool _showAppliedEffects = true;
        private Rect _minimizeRect;
        
        private void OnValidate()
        {
            if (_owner == null)
            {
                _owner = GetComponent<AbilitySystemBehaviour>();
            }
        }

        private void Start()
        {
            _minimizeRect = _windowRect;
            _attributeSystem = _owner.AttributeSystem;
        }

        private void OnGUI()
        {
            if (_owner == null) return;
            // DragWindow
            Rect newRect;
            Rect inputRect = _showDebug ? _windowRect : _minimizeRect;

            GUILayout.BeginVertical();
            newRect = GUILayout.Window(_windowId, inputRect, DoMyWindow, $"{_owner.name} Stats", GUILayout.ExpandHeight(true));
            GUILayout.EndVertical();

            if (!_showDebug)
                _windowRect.position = newRect.position;
            else
                _windowRect = newRect;
        }

        private void DoMyWindow(int windowID)
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
                {
                    computedValue = abstractEffect.EffectSO.EffectDetails.Modifiers[0].ModifierComputationMethod
                        .CalculateMagnitude(abstractEffect);
                }
                string details = "";
                if (abstractEffect.Parameters != null)
                {
                    var skillParams = (SkillParameters) abstractEffect.Parameters;
                    details = $"\nTurns: {skillParams.ContinuesTurn} | Base Power: {skillParams.BasePower}";
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
    #endif
    }
}