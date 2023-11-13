using System.Collections.Generic;
using CryptoQuest.Character.Hero.AvatarProvider;
using CryptoQuest.Church.UI;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Church
{
    public class ChurchPresenter : MonoBehaviour
    {
        [SerializeField] private List<UICharacter> _uiListCharacter;
        private IPartyController _partyController;
        private IHeroAvatarProvider _heroAvatarProvider;

        private void OnEnable()
        {
            Init();
        }
        
        private void Init()
        {
            _partyController = ServiceProvider.GetService<IPartyController>();
            _heroAvatarProvider = GetComponent<IHeroAvatarProvider>();
            GetPartyCharacter();
        }

        private void GetPartyCharacter()
        {
            for (int i = 0; i < _partyController.Slots.Length; i++)
            {
                _uiListCharacter[i].gameObject.SetActive(true);
                _uiListCharacter[i].ConfigureCharacter(_partyController.Slots[i].HeroBehaviour, _heroAvatarProvider);
            }
        }
    }
}
