using System.Collections.Generic;
using CryptoQuest.Character.Attributes;
using IndiGames.Core.Events.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using UnityEngine.AddressableAssets;
using CryptoQuest.Gameplay.Battle.Core.Components;
using UnityEngine;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Gameplay.PlayerParty.Helper;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;

namespace CryptoQuest.EditorTool
{
    public class BattleEditorCheat : MonoBehaviour
    {
        [SerializeField] private PartySO _party;
        [SerializeField] private BattleLoader _battleLoader;
        [SerializeField] private BattleDataSO[] _battleDataSOs;
        [SerializeField] private AssetReferenceT<Sprite> _defaultBackground;
        [SerializeField] private AttributeScriptableObject _attackSo;
        [SerializeField] private AbilityScriptableObject _buffAttackAbility;

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
        private bool _enableOneHitCheat = false;
        private float baseDamageValue = 9;
        private Dictionary<AbilitySystemBehaviour, GameplayAbilitySpec> _memberAbilityDict = new();

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
            newRect = GUILayout.Window(_windowId, inputRect, DoMyWindow, $"Battle Simulate",
                GUILayout.ExpandHeight(true));
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
                RenderDisableLoadBattle();
                RenderBattleToLoad();
                RenderPlayerParty();
                RenderOneHitCheat();
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
        }

        private void RenderDisableLoadBattle()
        {
            _battleLoader.enabled =
                GUILayout.Toggle(_battleLoader.enabled, "Disable Battle Loader", GUILayout.Width(_guiWidth));
        }

        private void RenderOneHitCheat()
        {
            var playerTeam = _party.PlayerTeam;
            var label = _enableOneHitCheat ? "Disable" : "Enable";
            if (GUILayout.Button($"{label} One hit cheat"))
            {
                foreach (var member in playerTeam.Members)
                {
                    if (!member.gameObject.activeSelf) continue;
                    if (!_enableOneHitCheat)
                    {
                        var ability = _buffAttackAbility.GetAbilitySpec(member);
                        _memberAbilityDict[member] = ability;
                        ability.ActivateAbility();
                        // TODO: REFACTOR ATTRIBUTE SYSTEM
                    }
                    else
                    {
                        var ability = _memberAbilityDict[member];
                        ability.OnAbilityRemoved(ability);
                        // TODO: REFACTOR ATTRIBUTE SYSTEM
                    }
                }

                _enableOneHitCheat = !_enableOneHitCheat;
            }
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

            var playerTeam = _party.PlayerTeam;
            for (int i = 0; i < playerTeam.Members.Count; i++)
            {
                var member = playerTeam.Members[i];
                var memberGO = playerTeam.Members[i].gameObject;
                if (member == _party.MainSystem) continue;
                var activeStatusLabel = memberGO.activeSelf ? "Disable" : "Active";
                if (GUILayout.Button($"{activeStatusLabel} member {i}"))
                {
                    memberGO.SetActive(!memberGO.activeSelf);
                    var destination = memberGO.activeSelf ? 1 : playerTeam.Members.Count - 1;
                    playerTeam.Members.SortElement(i, destination);
                    break;
                }
            }
        }

#endif
    }
}