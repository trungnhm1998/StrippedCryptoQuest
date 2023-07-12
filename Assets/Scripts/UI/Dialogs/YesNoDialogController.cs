using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using UnityEngine;

namespace CryptoQuest.UI.Dialogs
{
    public class YesNoDialogController : AbstractDialogController<UIYesNoDialog>
    {
        [SerializeField] private DialogueEventChannelSO _dialogueEventSO;

        protected override void RegisterEvents()
        {
            throw new global::System.NotImplementedException();
        }

        protected override void UnregisterEvents()
        {
            throw new global::System.NotImplementedException();
        }

        protected override void SetupDialog(UIYesNoDialog dialog)
        {
            dialog.Show();
        }
    }
}
