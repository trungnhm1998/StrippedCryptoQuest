using CryptoQuest.Events;
using CryptoQuest.Item.Ocarinas.Data;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using IndiGames.Core.Events.ScriptableObjects;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using UnityEngine.AddressableAssets;
using CryptoQuest.Gameplay.Battle.Core.Components;
using UnityEngine;

namespace CryptoQuest.EditorTool
{
    public class BattleEditorCheat : MonoBehaviour
    {
        [SerializeField] private GameplayBus _gameplayBus;
        [SerializeField] private BattleDataSO[] _battleDataSOs;
        [SerializeField] private SceneScriptableObject[] _battleScenes;
        [SerializeField] private AssetReferenceT<Sprite> _defaultBackground;

        [Header("Raise Event")]
        [SerializeField] private TriggerBattleEncounterEventSO _triggerBattleEncounterEvent;

        [Header("Listen Event")]
        [SerializeField] private VoidEventChannelSO _enterBattleChannelEvent;
        [SerializeField] private VoidEventChannelSO _enterFieldChannelEvent;

        [Header("Debug config")]
        [SerializeField] private float _guiWidth = 400;
        [SerializeField] private bool _showDebug = false;
        [SerializeField] private Rect _windowRect = new Rect(20, 20, 120, 50);

        private Rect _minimizeRect;
        private bool _showBattle = true;
        private bool _showParty = true;
        private bool _disableDebug = false;
        private int _windowId;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private void Start()
        {
            _minimizeRect = _windowRect;
            _windowId = gameObject.GetInstanceID();
        }

        private void OnEnable()
        {
            _enterBattleChannelEvent.EventRaised += EnterBattle;
            _enterFieldChannelEvent.EventRaised += EnterField;
        }

        private void OnDisable()
        {
            _enterBattleChannelEvent.EventRaised -= EnterBattle;
            _enterFieldChannelEvent.EventRaised -= EnterField;
        }

        private void EnterBattle()
        {
            _disableDebug = true;
        }

        private void EnterField()
        {
            _disableDebug = false;
        }

        private void OnGUI()
        {
            if (_disableDebug) return;
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
            GUILayout.BeginHorizontal("box", GUILayout.Width(_guiWidth));
            {
                GUILayout.BeginVertical();
                RenderBattleToLoad();
                RenderPlayerParty();
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
        }

        private void RenderBattleToLoad()
        {
            _showBattle =
                GUILayout.Toggle(_showBattle, "Show Battles", GUILayout.Width(_guiWidth));
            if (!_showBattle) return;

            foreach (var data in _battleDataSOs)
            {
                if (!GUILayout.Button($"Load battle: {data.name}")) continue;
                _showDebug = false;
                
                var battleInfo = new BattleInfo(data, true, _defaultBackground);
            _triggerBattleEncounterEvent.Raise(battleInfo);
            }
        }

        private void RenderPlayerParty()
        {
            _showParty =
                GUILayout.Toggle(_showParty, "Show Party", GUILayout.Width(_guiWidth));
            if (!_showParty) return;

            var playerTeam = _gameplayBus.PlayerTeam;
            for (int i = 0; i < playerTeam.Members.Count; i++)
            {
                var member = playerTeam.Members[i];
                var memberGO = playerTeam.Members[i].gameObject;
                if (member == _gameplayBus.MainSystem) continue;
                var activeStatusLabel = memberGO.activeSelf ? "Disable" : "Active"; 
                if (GUILayout.Button($"{activeStatusLabel} member {i}"))
                {
                    memberGO.SetActive(!memberGO.activeSelf);
                }
            }
        }

#endif
    }
}