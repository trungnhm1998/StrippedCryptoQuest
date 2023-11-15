using System.Collections.Generic;
using CryptoQuest.Character.Hero.AvatarProvider;
using CryptoQuest.Church.Interface;
using CryptoQuest.Church.UI;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Input;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Church
{
    [RequireComponent(typeof(IPartyProvider))]
    public class ChurchPresenter : MonoBehaviour
    {
        [SerializeField] private List<UICharacter> _uiListCharacter;
        [SerializeField] private MerchantsInputManager _input;
        [SerializeField] private UICurrency _currency;
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _gold;
        private IPartyReviver _partyReviver;
        private IPartyController _partyController;
        private IHeroAvatarProvider _heroAvatarProvider;
        public UICharacter CharacterToRevive { get; private set; }
        public bool IsEnoughGold { get; private set; }
        public bool IsAlive { get; private set; }
        private float _currentGold;

        private void OnEnable()
        {
            Init();
        }

        public void UpdateCurrency()
        {
            _currentGold = _wallet[_gold].Amount;
            _currency.UpdateCurrency(_currentGold);
        }

        private void Init()
        {
            _input.EnableInput();
            _partyController = ServiceProvider.GetService<IPartyController>();
            _partyReviver = GetComponent<IPartyReviver>();
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
            CharacterToRevive = character;
        }

        public void SelectDefaultButton()
        {
            if (_partyController.Slots.Length > 0)
            {
                _uiListCharacter[0].CharacterButton.Select();
            }
        }

        public void ValidateGoldToRevive(float cost)
        {
            IsEnoughGold = _currentGold >= cost;
            if (IsEnoughGold)
                ReviveCharacter(cost);
        }

        private void ReviveCharacter(float cost)
        {
            IsAlive = CharacterToRevive.HeroBehaviour.IsValidAndAlive();
            if (!IsAlive)
            {
                _partyReviver.ReviveCharacter(CharacterToRevive);
                _wallet[_gold].SetAmount(_currentGold - cost);
                UpdateCurrency();
            }
        }
    }
}
