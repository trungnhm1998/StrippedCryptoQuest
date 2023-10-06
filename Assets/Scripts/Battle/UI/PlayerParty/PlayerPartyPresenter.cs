using System.Collections;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Character.Hero.AvatarProvider;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.Core.Events.ScriptableObjects;
using TinyMessenger;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Battle.UI.PlayerParty
{
    public class PlayerPartyPresenter : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;
        [SerializeField] private UICharacterBattleInfo[] _characterUis;
        public UICharacterBattleInfo[] CharacterUIs => _characterUis;

        private IPartyController _party;
        private IHeroAvatarProvider _heroAvatarProvider;
        private TinyMessageSubscriptionToken _token;

        private void Awake()
        {
            _token = BattleEventBus.SubscribeEvent<UnloadingEvent>(UnloadAvatars);
            _sceneLoadedEvent.EventRaised += LoadPlayerPartyUI;
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_token);
            _sceneLoadedEvent.EventRaised -= LoadPlayerPartyUI;
        }

        /// <summary>
        /// If starting with correct flow, this method could be called using Awake but, party only loaded after
        /// GameplayManager scene loaded
        /// </summary>
        private void LoadPlayerPartyUI()
        {
            _heroAvatarProvider = GetComponent<IHeroAvatarProvider>();
            _party = ServiceProvider.GetService<IPartyController>();
            LoadHeroesUI();
        }

        private void LoadHeroesUI()
        {
            for (var index = 0; index < _party.Slots.Length; index++)
            {
                var slot = _party.Slots[index];
                var characterUI = _characterUis[index];
                characterUI.gameObject.SetActive(slot.IsValid());
                if (slot.IsValid() == false) continue;

                characterUI.Init(slot.HeroBehaviour);
            }

            if (_heroAvatarProvider == null) return;
            for (var index = 0; index < _party.Slots.Length; index++)
            {
                var slot = _party.Slots[index];
                if (slot.IsValid() == false) continue;
                StartCoroutine(CoLoadBattleAvatar(slot.HeroBehaviour, _characterUis[index]));
            }
        }

        private IEnumerator CoLoadBattleAvatar(HeroBehaviour hero, UICharacterBattleInfo characterUI)
        {
            yield return _heroAvatarProvider.LoadAvatarAsync(hero);
            characterUI.SetBattleAvatar(hero.BattleAvatar);
        }

        private void UnloadAvatars(UnloadingEvent _)
        {
            foreach (var slot in _party.Slots)
            {
                if (slot.HeroBehaviour && slot.HeroBehaviour.BattleAvatar)
                    Addressables.Release(slot.HeroBehaviour.BattleAvatar);
            }
        }
    }
}