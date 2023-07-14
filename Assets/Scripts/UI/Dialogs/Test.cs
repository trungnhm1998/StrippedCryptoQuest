using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.UI.Dialogs.BattleDialog
{
    public class Test : MonoBehaviour
    {
        private const string SUBJECT_ARGUMENT = "subject";
        public TMP_Text text;
        [field: SerializeField] public string DefaultCharacterName { get; private set; }
        private LocalizedString _charName;
        [SerializeField] private LocalizeStringEvent stringEvent;

        private void OnDisable()
        {
            _charName.StringChanged -= LocalizeCharacterName;
        }

        public void SetName(LocalizedString name)
        {
            _charName = name;
            _charName.StringChanged += LocalizeCharacterName;
            LocalizeCharacterName(name.GetLocalizedString());
        }

        private void LocalizeCharacterName(string value)
        {
            DefaultCharacterName = value;
            stringEvent.StringReference.RefreshString();
        }
    }
}
