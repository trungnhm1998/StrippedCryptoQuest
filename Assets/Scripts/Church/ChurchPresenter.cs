using System.Collections.Generic;
using CryptoQuest.Character.Hero.AvatarProvider;
using CryptoQuest.Church.UI;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Input;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.Church
{
    public class ChurchPresenter : MonoBehaviour
    {
        [SerializeField] private GameplayEffectDefinition gameplayEffectDefinition;
        [SerializeField] private List<UICharacter> _uiListCharacter;
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

        public void ReviveCharacter(UICharacter character)
        {
            AbilitySystemBehaviour abilitySystem = character.HeroBehaviour.GetComponent<AbilitySystemBehaviour>();
            if (!character.HeroBehaviour.IsValidAndAlive())
            {
                abilitySystem.ApplyEffectSpecToSelf(abilitySystem.MakeOutgoingSpec(gameplayEffectDefinition));
            }
        }

        public void SelectDefaultButton()
        {
            if (_partyController.Slots.Length > 0)
            {
                _uiListCharacter[0].CharacterButton.Select();
            }
        }
    }
}
