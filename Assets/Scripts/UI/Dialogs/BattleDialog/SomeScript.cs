using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

namespace CryptoQuest.UI.Dialogs.BattleDialog
{
    public class SomeScript : MonoBehaviour
    {
        public LevelStatusDialogEventChannelSO _rewardDialogEvent;
        public LevelStatusDialogData _rewardDialogData;
        public LocalizedString str;
        public LocalizedString input;
        public TMP_Text text;
        // Start is called before the first frame update
        void Start()
        {
            // CHAR_PRIEST
            // var dict = new Dictionary<string, string>() { { "subject", input.GetLocalizedString() } };
            // str.Arguments = new object[] { dict };

            // str.StringChanged += (string value) =>
            // {
            //     Debug.Log(value);
            //     text.text = value;
            // };

            _rewardDialogEvent.Show(_rewardDialogData);

        }
        
    }
}
