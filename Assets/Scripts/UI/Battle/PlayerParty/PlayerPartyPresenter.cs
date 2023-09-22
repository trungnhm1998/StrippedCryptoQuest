using System.Collections;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Battle.PlayerParty
{
    public class PlayerPartyPresenter : MonoBehaviour
    {
        [SerializeField] private BattleAvatarDatabase _avatarDatabase;
        [SerializeField] private UICharacterBattleInfo[] _characterUis;
        public UICharacterBattleInfo[] CharacterUIs => _characterUis;

        private IParty _party;

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
            _party = ServiceProvider.GetService<IPartyController>().Party;
            StartCoroutine(CoLoadPartyMembers());
        }

        private IEnumerator CoLoadPartyMembers()
        {
            for (var index = 0; index < _party.Members.Length; index++)
            {
                var member = _party.Members[index];
                var characterUI = _characterUis[index];
                characterUI.gameObject.SetActive(member.IsValid());
                if (!characterUI.gameObject.activeSelf) continue;

                characterUI.Init(member);
                yield return CoLoadBattleAvatar(member, characterUI);
            }
        }

        private IEnumerator CoLoadBattleAvatar(CharacterSpec character, UICharacterBattleInfo characterUI)
        {
            var id = $"{character.BackgroundInfo.Label.labelString}_{character.Class.Label.labelString}";
            yield return _avatarDatabase.LoadDataById(id);
            characterUI.SetBattleAvatar(_avatarDatabase.GetDataById(id));
        }
    }
}