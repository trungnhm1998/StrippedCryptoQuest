using CryptoQuest.Events;
using CryptoQuest.Item.Ocarinas.Data;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.EditorTool
{
    public class LoadBattleEditorCheat : MonoBehaviour
    {
        [SerializeField] private SceneScriptableObject[] _battleScenes;
        [SerializeField] private LoadSceneEventChannelSO _loadBattleSceneEvent;

        [SerializeField] private float _guiWidth = 400;
        [SerializeField] private float _guiButtonHeight = 50;
        [SerializeField] private int _fontSize = 20;
        [SerializeField] private bool _showDebug = false;
        [SerializeField] private Rect _windowRect = new Rect(20, 20, 120, 50);
        [SerializeField] private int _windowId;

        private Rect _minimizeRect;

        private bool _showBattle = true;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private void Start()
        {
            _minimizeRect = _windowRect;
        }

        private void OnGUI()
        {
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
            _showBattle =
                GUILayout.Toggle(_showBattle, "Show Battles", GUILayout.Width(_guiWidth));

            if (!_showBattle) return;
            GUI.skin.label.fontSize = _fontSize;
            foreach (var scene in _battleScenes)
            {
                if (!GUILayout.Button($"Load battle scene: {scene.name}",
                        GUILayout.Width(_guiWidth),
                        GUILayout.Height(_guiButtonHeight))) continue;
                _showDebug = false;
                _loadBattleSceneEvent.LoadingRequested(scene);
            }
        }
#endif
    }
}