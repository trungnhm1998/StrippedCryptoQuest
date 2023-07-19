using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Dialogs.LevelStatusDialog
{
    public class LevelUpCharName : MonoBehaviour
    {
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
