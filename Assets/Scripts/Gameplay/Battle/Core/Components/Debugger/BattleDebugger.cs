using CryptoQuest.Events;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.Debugger
{
    public class BattleDebugger : MonoBehaviour
    {
    #if UNITY_EDITOR || DEVELOPMENT_BUILD 
        [SerializeField] private BattleManager _battleManager;
        [SerializeField] private StringEventChannelSO _gotNewLogEventChannel;
        [SerializeField] private VoidEventChannelSO _turnEndEventChannel;
        [SerializeField] private bool _showDebug = false;
        [SerializeField] private Rect _windowRect = new Rect(20, 20, 120, 50);
        [SerializeField] private float _windowWidth = 400;
        [SerializeField] private int _windowId;

        private AttributeSystemBehaviour _attributeSystem;
        private bool _showActions = true;
        private bool _showLogs = true;
        private Rect _minimizeRect;
        private bool _showTargetUnit = false;
        private string _currentLog;
        
        private void OnValidate()
        {
            if (_battleManager == null)
            {
                _battleManager = GetComponent<BattleManager>();
            }
        }
        
        private void Start()
        {
            _minimizeRect = _windowRect;
        }

        private void OnEnable()
        {
            _gotNewLogEventChannel.EventRaised += OnGotNewLog;
            _turnEndEventChannel.EventRaised += OnTurnEnd;
        }

        private void OnDisable()
        {
            _gotNewLogEventChannel.EventRaised -= OnGotNewLog;
            _turnEndEventChannel.EventRaised -= OnTurnEnd;
        }
        
        private void OnGotNewLog(string log)
        {
            _showLogs = true;
            _currentLog = log;
        }

        private void OnTurnEnd()
        {
            _showActions = true;
            _currentLog = $"Current Turn: {_battleManager.Turn}";
        }

        private void OnGUI()
        {
            if (_battleManager == null) return;
            // DragWindow
            Rect newRect;
            Rect inputRect = _showDebug ? _windowRect : _minimizeRect;

            GUILayout.BeginVertical();
            newRect = GUILayout.Window(_windowId, inputRect, DoMyWindow, $"Battle Simulate", GUILayout.ExpandHeight(true));
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
                RenderBattleActions();
                RenderSelectTargets();
                RenderBattleLog();
            }
            GUILayout.EndHorizontal();
        }

        private void RenderBattleLog()
        {
            _showLogs = GUILayout.Toggle(_showLogs, "Show battle logs");
            if (!_showLogs) return;
            GUILayout.BeginVertical();
            GUILayout.Label(_currentLog);
            GUILayout.EndVertical();
        }

        private void RenderBattleActions()
        {
            if (!_showActions) return;
            GUILayout.BeginVertical();
            AbilitySystemBehaviour currentUnitOwner = _battleManager.CurrentUnit.Owner;
            if (currentUnitOwner == null) return;
            foreach (AbstractAbility skill in currentUnitOwner.GrantedAbilities.Abilities)
            {
                GUI.enabled = !skill.IsActive;
                if (GUILayout.Button($"Select skill {skill.AbilitySO.name}"))
                {
                    _battleManager.CurrentUnit.SelectAbility(skill);
                    _showTargetUnit = true;
                    _showActions = false;
                }
                GUI.enabled = true;
            }
            GUILayout.EndVertical();
        }
        
        private void RenderSelectTargets()
        {
            IBattleUnit currentUnit = _battleManager.CurrentUnit;
            BattleTeam currentUnitOpponent = currentUnit.OpponentTeam;

            if (!_showTargetUnit || currentUnitOpponent == null) return;
            GUILayout.BeginVertical();
            GUILayout.Label("Select a target");
            foreach (AbilitySystemBehaviour target in currentUnitOpponent.Members)
            {
                if (target == null) continue;
                var targetUnit = target.GetComponent<IBattleUnit>();
                var buttonLabel = $"Target {targetUnit.UnitInfo.DisplayName}";
                if (GUILayout.Button(buttonLabel))
                {
                    currentUnit.SelectSingleTarget(target);
                    _showTargetUnit = false;
                }
                GUI.enabled = true;
            }
            GUILayout.EndVertical();
        }
    #endif
    }
}