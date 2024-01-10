using CryptoQuest.UI.Dialogs.BattleDialog;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Dialogs.DialogWithCharacterName
{
    public class UICharacterNamedDialog : UIGenericDialog
    {
        [SerializeField] private TMP_Text _characterText;
        [SerializeField] private LocalizeStringEvent _characterLocalizeStringEvent;

        public UIGenericDialog WithHeader(LocalizedString characterName)
        {
            _characterLocalizeStringEvent.StringReference = characterName;
            return this;
        }

        public override void Clear()
        {
            base.Clear();
            _characterText.text = string.Empty;
        }
    }
}