using CryptoQuest.Beast;
using CryptoQuest.Beast.Avatar;
using CryptoQuest.UI.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Beast.UI
{
    public class UIBeastDetail : MonoBehaviour
    {
        [SerializeField] private BeastAvatarSO _database;
        [SerializeField] private LocalizeStringEvent _beastName;
        [SerializeField] private LocalizeStringEvent _localizedPassiveSkill;
        [SerializeField] private TMP_Text _txtPassiveSkill;
        [SerializeField] private Image _beastImage;
        [SerializeField] private Image _beastElement;

        [SerializeField] private BeastEventChannel _showBeastDetailsEventChannel;

        private void OnEnable()
        {
            _showBeastDetailsEventChannel.EventRaised += FillUI;
        }

        private void OnDisable()
        {
            _showBeastDetailsEventChannel.EventRaised -= FillUI;
        }

        public void FillUI(IBeast beast)
        {
            _txtPassiveSkill.text = "";
            _beastName.StringReference = beast.LocalizedName;

            _localizedPassiveSkill.StringReference =
                beast.Passive != null ? beast.Passive.Description : new LocalizedString();
            _localizedPassiveSkill.RefreshString();

            _beastElement.sprite = beast.Elemental.Icon;

            SetAvatar(beast);
        }

        private void SetAvatar(IBeast beast)
        {
            // var id = $"{beast.Id}-{beast.Elemental.Id}-{beast.Class.Id}";
            // var assetRefAvatar = _database.CacheLookupTable[id];
            // if (assetRefAvatar == null || !assetRefAvatar.RuntimeKeyIsValid())
            // {
            //     _beastImage.enabled = false;
            //     return;
            // }
            //
            // _beastImage.LoadSpriteAndSet(assetRefAvatar);
        }
    }
}