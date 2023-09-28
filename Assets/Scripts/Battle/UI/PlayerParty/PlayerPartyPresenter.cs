using System.Collections;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero.AvatarProvider;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Battle.UI.PlayerParty
{
    public class PlayerPartyPresenter : MonoBehaviour
    {
        [SerializeField] private UICharacterBattleInfo[] _characterUis;
        public UICharacterBattleInfo[] CharacterUIs => _characterUis;

        private IPartyController _party;
        private IHeroAvatarProvider _heroAvatarProvider;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _characterUis = GetComponentsInChildren<UICharacterBattleInfo>(true);
        }
#endif

        private void Awake()
        {
            InitParty();
            _heroAvatarProvider = GetComponent<IHeroAvatarProvider>();
        }

        private void InitParty()
        {
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
    }
}