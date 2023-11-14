using System;
using System.Collections.Generic;
using CryptoQuest.Character.Hero.AvatarProvider;
using CryptoQuest.Church.UI;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Input;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Church
{
    public class ChurchPresenter : MonoBehaviour
    {
        public event Action<bool> IsReviveSuccessEvent;
        [SerializeField] private GameplayEffectDefinition _reviveEffectDefinition;
        [SerializeField] private List<UICharacter> _uiListCharacter;
        [SerializeField] private UICharacter _characterToRevive;
        [SerializeField] private MerchantsInputManager _input;
        private IPartyController _partyController;
        private IHeroAvatarProvider _heroAvatarProvider;

        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            _input.EnableInput();
            _partyController = ServiceProvider.GetService<IPartyController>();
            _heroAvatarProvider = GetComponent<IHeroAvatarProvider>();
            GetPartyCharacter();
            EventSystem.current.SetSelectedGameObject(_uiListCharacter[0].gameObject);
        }

        private void GetPartyCharacter()
        {
            for (int i = 0; i < _partyController.Slots.Length; i++)
            {
                _uiListCharacter[i].gameObject.SetActive(true);
                _uiListCharacter[i].ConfigureCharacter(_partyController.Slots[i].HeroBehaviour, _heroAvatarProvider);
            }
        }

        public void GetCharacter(UICharacter character)
        {
            _characterToRevive = character;
        }

        public void SelectDefaultButton()
        {
            if (_partyController.Slots.Length > 0)
            {
                _uiListCharacter[0].CharacterButton.Select();
            }
        }

        public void ReviveCharacter()
        {
            AbilitySystemBehaviour abilitySystem = _characterToRevive.HeroBehaviour.GetComponent<AbilitySystemBehaviour>();
            bool isAlive = _characterToRevive.HeroBehaviour.IsValidAndAlive();
            IsReviveSuccessEvent?.Invoke(isAlive);
            if (!isAlive)
                abilitySystem.ApplyEffectSpecToSelf(abilitySystem.MakeOutgoingSpec(_reviveEffectDefinition));

        }
    }
}
