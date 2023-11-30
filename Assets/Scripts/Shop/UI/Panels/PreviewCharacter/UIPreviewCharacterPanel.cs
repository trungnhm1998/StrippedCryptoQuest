using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using System.Collections.Generic;
using System.Linq;
using IndiGames.Core.Common;
using UnityEngine;
using UnityEngine.Pool;

namespace CryptoQuest.Shop.UI.Panels.PreviewCharacter
{
    public class UIPreviewCharacterPanel : MonoBehaviour, IPreviewCharacter
    {
        [SerializeField] private Transform _previewCharacteGroup;
        [SerializeField] private UIPreviewCharacterInfo _uiPreviewCharacter;
        [SerializeField] private IPartyController _party;

        private List<UIPreviewCharacterInfo> _characters = new();
        private IObjectPool<UIPreviewCharacterInfo> _charPool;

        private void Awake()
        {
            _party = ServiceProvider.GetService<IPartyController>();
            _charPool ??= new ObjectPool<UIPreviewCharacterInfo>(OnCreate, OnGet,
                OnRelease, OnDestroyPool);
        }

        private void OnEnable()
        {
            ReleaseAll();
            _characters.Clear();
            foreach (var character in _party.Slots)
            {
                if(character.IsValid())
                {
                    var charInfo = _charPool.Get();
                    charInfo.Init(character.HeroBehaviour);
                    _characters.Add(charInfo);
                }    
            }
        }

        private void ReleaseAll()
        {
            foreach (var character in _characters)
            {
                _charPool.Release(character);
            }
        }    

        public void Preview(EquipmentInfo equipmentInfo)
        {
            foreach (var character in _characters)
            {
                character.Preview(equipmentInfo);
            }
        }

        private UIPreviewCharacterInfo OnCreate()
        {
            var preview = Instantiate(_uiPreviewCharacter, _previewCharacteGroup);
            return preview;
        }

        private void OnGet(UIPreviewCharacterInfo obj)
        {
            obj.transform.SetAsLastSibling();
            obj.gameObject.SetActive(true);
        }

        private void OnRelease(UIPreviewCharacterInfo obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnDestroyPool(UIPreviewCharacterInfo obj)
        {
            Destroy(obj.gameObject);
        }
    }
}
