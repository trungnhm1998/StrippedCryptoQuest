using System;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero.AvatarProvider;
using CryptoQuest.UI.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Home.UI.CharacterList
{
    public class UICharacterDetails : MonoBehaviour
    {
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;

        [Header("UI References")]
        [SerializeField] private Image _avatar;
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private Image _characterElement;
        [SerializeField] private UIAttributeBar _expBar;

        private AttributeSystemBehaviour _inspectingAttributeSystem;
        [SerializeField] private string _lvlTxtFormat = "Lv. {0}";

        [SerializeField] private HeroAvatarDatabase _avatarDatabase;
        private AsyncOperationHandle<Sprite> _handle;

        public void InspectCharacter(HeroBehaviour hero)
        {
            if (hero.IsValid() == false) return;
            _inspectingAttributeSystem = hero.GetComponent<AttributeSystemBehaviour>();
            _attributeChangeEvent.AttributeSystemReference = _inspectingAttributeSystem;
            SetupUI(hero);
        }

        private void SetupUI(HeroBehaviour hero)
        {
            hero.TryGetComponent(out LevelSystem levelSystem);
            SetElement(hero.Spec.Elemental.Icon);
            SetLevel(levelSystem.Level);
            SetLocalizedName(hero.DetailsInfo.LocalizedName);
            SetAvatarAsync(hero);
            SetupExpUI(hero);
        }

        private void SetupExpUI(HeroBehaviour hero)
        {
            if (!hero.TryGetComponent<LevelSystem>(out var levelSystem)) return;
            SetMaxExp(levelSystem.GetNextLevelRequireExp());
            SetExp(levelSystem.GetCurrentLevelExp());
        }

        #region Setup UI

        private void SetLocalizedName(LocalizedString localizedName) => _localizedName.StringReference = localizedName;

        private void SetName(string charName) => _name.text = charName;

        private void SetAvatarAsync(HeroBehaviour hero)
        {
            if (_handle.IsValid())
            {
                _handle.Completed -= SetAvatarInternal;
                _avatar.enabled = false;
                Addressables.Release(_handle);
                _handle = new();
            }

            var id = $"{hero.DetailsInfo.Id}-{hero.Class.Id}";
            try
            {
                var avatar = _avatarDatabase.CacheLookupTable[id];
                if (avatar.OperationHandle.IsValid())
                {
                    SetAvatar(avatar.OperationHandle.Result as Sprite);
                    return;
                }

                _handle = avatar.LoadAssetAsync();
                _handle.Completed += SetAvatarInternal;
            }
            catch (Exception e)
            {
                _avatar.enabled = false;
            }
        }

        private void OnDisable()
        {
            if (_handle.IsValid()) _handle.Completed -= SetAvatarInternal;
        }

        private void SetAvatarInternal(AsyncOperationHandle<Sprite> _) => SetAvatar(_handle.Result);

        private void SetAvatar(Sprite sprite)
        {
            _avatar.enabled = true;
            _avatar.sprite = sprite;
        }

        private void SetElement(Sprite elementIcon) => _characterElement.sprite = elementIcon;

        private void SetExp(float exp) => _expBar.SetValue(exp);

        private void SetMaxExp(int maxExp) => _expBar.SetMaxValue(maxExp);

        private void SetLevel(int lvl)
        {
            _level.text = string.Format(_lvlTxtFormat, lvl);
        }

        #endregion
    }
}