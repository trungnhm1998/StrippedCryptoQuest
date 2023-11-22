using System.Collections.Generic;
using System.Linq;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.ChangeClass.API;
using CryptoQuest.ChangeClass.View;
using CryptoQuest.Character.Hero;
using CryptoQuest.Character.Hero.AvatarProvider;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassSyncData : MonoBehaviour
    {
        [SerializeField] private List<Origin> Origins = new List<Origin>();
        [SerializeField] private List<Elemental> Elements = new List<Elemental>();
        [SerializeField] private HeroAvatarDatabase HeroAvatar;
        private LocalizedString _localizedName;
        private Sprite _element;
        private AssetReferenceT<Sprite> _avatar;

        public void SetClassMaterialData(UICharacter character)
        {
            SetCharacterData(character);
        }

        public void SetNewClassData(NewCharacter data, UIPreviewCharacter newCharacter)
        {
            SetCharacterData(data.characterId, data.elementId, newCharacter, data);
        }

        private void SetCharacterData(UICharacter character)
        {
            var characterId = character.Class.Origin.DetailInformation.Id;
            var classId = character.Class.Class.Id;
            
            string mapId = $"{characterId}-{classId}";
            var matchingAvatar = HeroAvatar.Maps.FirstOrDefault(avatar => avatar.Id == mapId);
            _avatar = matchingAvatar.Data;

            character.SyncAvatar(_avatar);
        }

        private void SetCharacterData(string characterId, string elementId, UIPreviewCharacter newCharacter, NewCharacter data)
        {
            var matchingOrigin = Origins.FirstOrDefault(origin => origin.DetailInformation.Id.ToString() == characterId);
            _localizedName = matchingOrigin?.DetailInformation.LocalizedName;

            var matchingElement = Elements.FirstOrDefault(element => elementId == element.Id.ToString());
            _element = matchingElement?.Icon;

            string mapId = $"{characterId}-{data.classId}";
            var matchingAvatar = HeroAvatar.Maps.FirstOrDefault(avatar => avatar.Id == mapId);
            _avatar = matchingAvatar.Data;

            newCharacter.PreviewNewCharacter(data, _localizedName, _element, _avatar);
        }

        public AssetReferenceT<Sprite> Avatar(UICharacter character, UIOccupation occupation)
        {
            string mapId = $"{character.Class.Origin.DetailInformation.Id}-{occupation.Class.CharacterClass.Id}";
            var matchingAvatar = HeroAvatar.Maps.FirstOrDefault(avatar => avatar.Id == mapId);
            return matchingAvatar.Data;
        }

        public void ReleaseAllAssetReference()
        {
            foreach (var data in HeroAvatar.Maps)
            {
                data.Data.ReleaseAsset();
            }
        }
    }
}