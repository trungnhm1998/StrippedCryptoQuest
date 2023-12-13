using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Beast;
using CryptoQuest.Beast.Avatar;
using CryptoQuest.UI.Extensions;
using CryptoQuest.UI.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Beast.UI
{
    public class UIBeastDetail : MonoBehaviour
    {
        [SerializeField] private GameObject _content;

        [Header("Configs")]
        [SerializeField] private BeastAvatarSO _database;

        [SerializeField] private LocalizeStringEvent _beastName;
        [SerializeField] private LocalizeStringEvent _localizedPassiveSkill;
        [SerializeField] private TMP_Text _txtPassiveSkill;
        [SerializeField] private TMP_Text _txtLevel;
        [SerializeField] private Image _beastImage;
        [SerializeField] private Image _beastElement;
        [SerializeField] private UIStars _stars;

        [Header("Events")]
        [SerializeField] private BeastEventChannel _showBeastDetailsEventChannel;

        private string _lvlFormat = string.Empty;

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
            SetPassiveSkill(beast.Passive);
            SetBeastName(beast.LocalizedName);
            SetBeastName(beast.LocalizedName);
            SetElement(beast.Elemental.Icon);
            SetStars(beast.Stars);
            SetLevel(beast.Level);
            SetAvatar(beast);
        }


        #region Setup

        private void SetAvatar(IBeast beast)
        {
            if (beast.Type == null ||
                beast.Elemental == null ||
                beast.Class == null)
            {
                Debug.LogWarning("UIBeastDetail::SetAvatar::Invalid Beast");
                return;
            }

            AssetReferenceT<Sprite> assetRefAvatar = null;
            foreach (var avatar in _database.AvatarMappings)
            {
                if (avatar.BeastId != beast.Type.BeastInformation.Id ||
                    avatar.ElementId != beast.Elemental.Id ||
                    avatar.ClassId != beast.Class.Id) continue;

                assetRefAvatar = avatar.Image;
                break;
            }

            if (assetRefAvatar == null || !assetRefAvatar.RuntimeKeyIsValid())
            {
                _beastImage.enabled = false;
                return;
            }

            _beastImage.LoadSpriteAndSet(assetRefAvatar);
        }

        private void SetPassiveSkill(PassiveAbility beastPassive)
        {
            _txtPassiveSkill.text = "";
            _localizedPassiveSkill.StringReference =
                beastPassive != null ? beastPassive.Description : new LocalizedString();
            _localizedPassiveSkill.RefreshString();
        }

        private void SetLevel(int value)
        {
            if (_lvlFormat == string.Empty)
            {
                _lvlFormat = _txtLevel.text;
            }

            _txtLevel.text = string.Format(_lvlFormat, value);
        }

        private void SetBeastName(LocalizedString beastName) => _beastName.StringReference = beastName;

        private void SetStars(int value) => _stars.SetStars(value);

        private void SetElement(Sprite element) => _beastElement.sprite = element;

        public void SetEnabled(bool isActive = true) => _content.SetActive(isActive);

        #endregion
    }
}