using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using UnityEngine;
using System.Collections;
using CryptoQuest.Gameplay.Character;

namespace CryptoQuest.UI.Battle.PlayerParty
{
    public class PlayerPartyPresenter : MonoBehaviour
    {
        [SerializeField] private BattleAvatarDatabase _avatarDatabase;
        [SerializeField] private ServiceProvider _serviceProvider;

        [SerializeField] private UICharacterBattleInfo[] _characterUis;
        public UICharacterBattleInfo[] CharacterUIs => _characterUis;

        private IParty _party;
        
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] private PartySO _testParty;

        private void OnValidate()
        {
            _characterUis = GetComponentsInChildren<UICharacterBattleInfo>(true);
        }
#endif

        private void Awake()
        {
            _party = _serviceProvider.PartyController?.Party;
            _serviceProvider.PartyProvided += InitParty;
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (_testParty) _party = _testParty;
#endif
            CoLoadPartyMembers();
        }

        private void OnDestroy()
        {
            _serviceProvider.PartyProvided -= InitParty;
        }

        private void InitParty(IPartyController partyController)
        {
            _party ??= _serviceProvider.PartyController.Party;
            CoLoadPartyMembers();
        }

        private void CoLoadPartyMembers()
        {
            StartCoroutine(LoadPartyMembers());
        }

        private IEnumerator LoadPartyMembers()
        {
            for (var index = 0; index < _party.Members.Length; index++)
            {
                var member = _party.Members[index];
                var characterUI = _characterUis[index];
                characterUI.gameObject.SetActive(member.IsValid());
                if (!characterUI.gameObject.activeSelf) continue;

                characterUI.Init(member);
                yield return LoadBattleAvatar(member, characterUI);
            }
        }

        private IEnumerator LoadBattleAvatar(CharacterSpec character, UICharacterBattleInfo characterUI)
        {
            var id = $"{character.BackgroundInfo.Label.labelString}_{character.Class.Label.labelString}";
            yield return _avatarDatabase.LoadDataById(id);
            characterUI.SetBattleAvatar(_avatarDatabase.GetDataById(id));
        }
    }
}
