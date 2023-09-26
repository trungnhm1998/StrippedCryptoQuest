using System.Collections;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Battle.UI.PlayerParty
{
    public class PlayerPartyPresenter : MonoBehaviour
    {
        [SerializeField] private BattleAvatarDatabase _avatarDatabase;
        [SerializeField] private UICharacterBattleInfo[] _characterUis;
        public UICharacterBattleInfo[] CharacterUIs => _characterUis;

        private IPartyController _party;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _characterUis = GetComponentsInChildren<UICharacterBattleInfo>(true);
        }
#endif

        private void Awake()
        {
            InitParty();
        }

        private void InitParty()
        {
            _party = ServiceProvider.GetService<IPartyController>();
            StartCoroutine(CoLoadPartyMembers());
        }

        private IEnumerator CoLoadPartyMembers()
        {
            for (var index = 0; index < _party.Slots.Length; index++)
            {
                var slot = _party.Slots[index];
                var characterUI = _characterUis[index];
                characterUI.gameObject.SetActive(slot.IsValid());
                if (slot.IsValid() == false) continue;

                characterUI.Init(slot.HeroBehaviour);
                yield return CoLoadBattleAvatar(slot.HeroBehaviour, characterUI);
            }
        }

        private IEnumerator CoLoadBattleAvatar(HeroBehaviour hero, UICharacterBattleInfo characterUI)
        {
            yield break; // TODO: REFACTOR CHARACTER
            // var id = $"{character.BackgroundInfo.Label.labelString}_{character.Class.Label.labelString}";
            // yield return _avatarDatabase.LoadDataById(id);
            // characterUI.SetBattleAvatar(_avatarDatabase.GetDataById(id));
        }
    }
}