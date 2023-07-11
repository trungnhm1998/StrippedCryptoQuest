using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Dialogs
{
    public class DialogueController : DialogController<UIDialogue>
    {
        [SerializeField] private DialogueEventChannelSO _dialogueEventSO;
        private DialogueScriptableObject _args;

        protected override void RegisterEvents()
        {
            _dialogueEventSO.ShowEvent += ShowDialog;
        }

        private void ShowDialog(DialogueScriptableObject arg0)
        {
            _args = arg0;
            LoadAssetDialog();
        }

        protected override void UnregisterEvents()
        {
            _dialogueEventSO.ShowEvent -= ShowDialog;
        }

        protected override void SetupDialog(UIDialogue dialog)
        {
            dialog
                .SetDialogue(_args)
                .Show();
        }
    }
}
