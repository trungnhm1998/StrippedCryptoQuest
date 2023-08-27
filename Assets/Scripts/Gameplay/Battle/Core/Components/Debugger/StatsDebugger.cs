using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.Debugger
{
    public class StatsDebugger : MonoBehaviour
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        [SerializeField] private AbilitySystemBehaviour _owner;
        [SerializeField] private bool _showDebug = false;
        [SerializeField] private Rect _windowRect = new Rect(20, 20, 120, 50);
        [SerializeField] private float _windowWidth = 400;

        private AttributeSystemBehaviour _attributeSystem;
        private bool _showAttributes = true;
        private bool _showAppliedEffects = true;
        private Rect _minimizeRect;
        private int _windowId;
        private IBattleUnit _unit;

        private void OnValidate()
        {
            if (_owner == null)
            {
                _owner = GetComponent<AbilitySystemBehaviour>();
            }
        }

        private void Start()
        {
            _unit = GetComponent<IBattleUnit>();
            _minimizeRect = _windowRect;
            _attributeSystem = _owner.AttributeSystem;
            _windowId = gameObject.GetInstanceID();
        }

        private void OnGUI()
        {
            if (_owner == null) return;
            // DragWindow
            Rect newRect;
            Rect inputRect = _showDebug ? _windowRect : _minimizeRect;

            GUILayout.BeginVertical();
            newRect = GUILayout.Window(_windowId, inputRect, DoMyWindow,
                $"{(_unit.UnitInfo?.DisplayName)} Stats", GUILayout.ExpandHeight(true));
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
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void RenderAttributesDebug()
        {
            _showAttributes = GUILayout.Toggle(_showAttributes, "Show Attributes");
            if (!_showAttributes) return;
            foreach (var attribute in _attributeSystem.AttributeValues)
            {
                GUILayout.Label($"{attribute.Attribute.name}: {attribute.CurrentValue}\n"
                                + $"(equipments [+{attribute.CoreModifier.Additive}] [*{attribute.CoreModifier.Multiplicative}]) "
                                + $"(skills/effects [+{attribute.ExternalModifier.Additive}] [*{attribute.ExternalModifier.Multiplicative}])");
            }
        }
#endif
    }
}