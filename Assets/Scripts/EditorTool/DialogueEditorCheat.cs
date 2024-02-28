﻿using CryptoQuest.Gameplay;
using CryptoQuest.System.Dialogue.YarnManager;
using UnityEngine;

namespace CryptoQuest.EditorTool
{
    public class DialogueEditorCheat : MonoBehaviour
    {
        [SerializeField] private LineView _lineView;
        [SerializeField] private GameStateSO _gameState;

        [Header("Debug")]
        [SerializeField] private bool _showDebug = false;

        [SerializeField] private string _nameToggle = "Auto Skip Dialogue";
        [SerializeField] private FontStyle _fontStyle;
        [SerializeField] private Font _font;
        [SerializeField] private int _fontSize;
        [SerializeField] private Color _colorText;
        [SerializeField] private Rect _debugRect = new Rect(Screen.width - 560, 20, 120, 20);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private void OnGUI()
        {
            bool isDialogueState = _gameState.CurrentGameState is EGameState.Dialogue or EGameState.Cutscene;

            var styleToggle = new GUIStyle(GUI.skin.toggle);
            styleToggle.fontSize = _fontSize;
            styleToggle.fontStyle = _fontStyle;
            styleToggle.font = _font;
            styleToggle.normal.textColor = _colorText;

            if (isDialogueState)
            {
                _showDebug = GUI.Toggle(_debugRect, _showDebug, _nameToggle, styleToggle);

                if (!_showDebug) return;
                _lineView.OnContinueClicked();
            }
        }
#endif
    }
}