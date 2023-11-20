using System;
using IndiGames.Core.Database;
using UnityEngine;
using UnityEngine.AddressableAssets;
using CryptoQuest.Character.Avatar;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CryptoQuest.Character.Hero.AvatarProvider
{
    public class HeroAvatarDatabase : AssetReferenceDatabaseT<string, Sprite>
    {
#if UNITY_EDITOR
        public enum EAvatar
        {
            HeroAvatar = 0,
            BattleAvatar = 1
        }

        [SerializeField] private EAvatar _typeOfAvatar;
        [SerializeField] private HeroAvatarSetSO _avatarSO;
        private const string AVATAR_PATH = "Assets/Arts/UI/CharactersAvatar";
        private const string BATTLE_PATH = "Assets/Arts/UI/Battle/Characters";
        private string _path;
        private int _classId;
        private int _characterId;

        public override void Editor_FetchDataInProject()
        {
            GetAssetPath();
            _maps = Array.Empty<Map>();
            var guids = AssetDatabase.FindAssets("t:sprite", new[] { _path });

            foreach (var guid in guids)
            {
                var instance = new Map();
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                if (Editor_Validate((asset, path)) == false) continue;
                var assetRef = new AssetReferenceT<Sprite>(guid);
                assetRef.SetEditorAsset(asset);
                GetAvatarId(asset);
                instance.Id = $"{_characterId}-{_classId}";
                instance.Data = assetRef;
                ArrayUtility.Add(ref _maps, instance);
                instance.Data.SetEditorSubObject(asset);
            }
        }

        private string GetAssetPath()
        {
            _path = _typeOfAvatar == EAvatar.HeroAvatar ? AVATAR_PATH : BATTLE_PATH;
            return _path;
        }

        private void GetAvatarId(Sprite sprite)
        {
            foreach (var avatar in _avatarSO.AvatarMappings)
            {
                if (avatar.ImageName == sprite.name + ".png")
                {
                    _characterId = avatar.CharacterId;
                    _classId = avatar.ClassId;
                    break;
                }
            }
        }
#endif
    }
}
