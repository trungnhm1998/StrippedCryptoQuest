using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero.AvatarProvider;
using CryptoQuest.Gameplay.PlayerParty;
using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.ShopSystem.PreviewCharacter
{
    public class CharacterPartyPresenter : MonoBehaviour
    {
        [SerializeField] private List<UICharacterInfo> _characterInfoUis;
        private IPartyController _partyController;
        private IHeroAvatarProvider _heroAvatarProvider;

        private void OnValidate()
        {
            _characterInfoUis =
                new List<UICharacterInfo>(GetComponentsInChildren<UICharacterInfo>(true));
        }

        private void OnEnable()
        {
            LoadPlayerPartyUI();
        }

        private void LoadPlayerPartyUI()
        {
            _heroAvatarProvider = GetComponent<IHeroAvatarProvider>();
            _partyController = ServiceProvider.GetService<IPartyController>();
            DisableCharacterObjects();
            LoadCharacterDetail();
        }

        private void DisableCharacterObjects()
        {
            for (int i = 0; i < _partyController.Slots.Length; i++)
            {
                _characterInfoUis[i].gameObject.SetActive(false);
            }
        }

        private void LoadCharacterDetail()
        {
            for (int i = 0; i < _partyController.Slots.Length; i++)
            {
                if (!_partyController.Slots[i].HeroBehaviour.IsValid()) return;
                _characterInfoUis[i].gameObject.SetActive(true);
                _characterInfoUis[i].LoadCharacterDetail(_partyController.Slots[i].HeroBehaviour);

                StartCoroutine(CoLoadAvatar(_partyController.Slots[i].HeroBehaviour, _characterInfoUis[i]));
            }
        }

        private IEnumerator CoLoadAvatar(HeroBehaviour hero, UICharacterInfo characterUI)
        {
            yield return _heroAvatarProvider.LoadAvatarAsync(hero);
            characterUI.SetAvatar(hero.Avatar);
        }
    }
}
