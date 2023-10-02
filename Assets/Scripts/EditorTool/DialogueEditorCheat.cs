using CryptoQuest.Gameplay;
using UnityEngine;

namespace CryptoQuest.EditorTool
{
    public class DialogueEditorCheat : MonoBehaviour
    {
        [SerializeField] private Yarn.Unity.LineView _lineView;
        [SerializeField] private GameStateSO _gameState;

        [Header("Debug config")]
        [SerializeField] private bool _showDebug = false;

        [SerializeField] private Rect _windowRect = new Rect(20, 80, 120, 50);

        private Rect _minimizeRect;
        private int _windowId;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private void Start()
        {
            _minimizeRect = _windowRect;
            _windowId = gameObject.GetInstanceID();
        }

        private void OnGUI()
        {
            Rect newRect;
            Rect innerRect = _showDebug ? _windowRect : _minimizeRect;

            GUILayout.BeginVertical();
            newRect = GUILayout.Window(_windowId, innerRect, DoMyWindow, $"Auto Skip Dialogue",
                GUILayout.ExpandHeight(true));
            GUILayout.EndVertical();

            if (!_showDebug)
                _windowRect.position = newRect.position;
            else
                _windowRect = newRect;
        }

        private void DoMyWindow(int id)
        {
            GUI.DragWindow(new Rect(0, 80, 200, 20));
            _showDebug = GUILayout.Toggle(_showDebug, "Auto Skip Dialogue");
            bool isDialogueState = _gameState.CurrentGameState == EGameState.Dialogue;

            if (_showDebug && isDialogueState)
            {
                _lineView.UserRequestedViewAdvancement();
            }
        }
#endif
    }
}