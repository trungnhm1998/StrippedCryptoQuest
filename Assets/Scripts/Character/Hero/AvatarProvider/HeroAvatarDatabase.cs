using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using IndiGames.Core.Database;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
        private const string AVATAR_PATH = "Assets/Arts/UI/CharactersAvatar";
        private const string BATTLE_PATH = "Assets/Arts/UI/Battle/Characters";
        private string _path;
        private int _classId;
        private List<int> _listAvatarId;

        public override void Editor_FetchDataInProject()
        {
            GetAssetPath();
            string pattern = @"\d+";
            _maps = Array.Empty<Map>();
            var classGuids = AssetDatabase.FindAssets("t:CharacterClass");
            var guids = AssetDatabase.FindAssets("t:sprite", new[] { _path });

            foreach (var guid in guids)
            {
                _listAvatarId = new();
                var instance = new Map();
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                if (Editor_Validate((asset, path)) == false) continue;
                var assetRef = new AssetReferenceT<Sprite>(guid);
                assetRef.SetEditorAsset(asset);
                MatchCollection matches = Regex.Matches(asset.name, pattern);
                foreach (Match match in matches)
                {
                    int result = int.Parse(match.Value);
                    _listAvatarId.Add(result);
                }
                GetClassId(classGuids);
                instance.Id = $"{_listAvatarId[0]}-{_classId}";
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

        private void GetClassId(string[] guids)
        {
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<CharacterClass>(path);
                char assetId = asset.Id.ToString().Reverse().Skip(1).FirstOrDefault();
                int classId = Convert.ToInt32(new string(assetId, 1));
                if (_listAvatarId[1] == classId)
                {
                    _classId = asset.Id;
                    break;
                }
            }
        }
#endif
    }
}
