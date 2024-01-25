using System.Collections;
using CryptoQuest.Character.Hero;
using CryptoQuest.Character.Hero.AvatarProvider;
using CryptoQuest.UI.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI
{
    public class UICharacterInfo : UICharacterListItem
    {
        [SerializeField] private GameObject _interactingIndicator;
        [SerializeField] private HeroAvatarDatabase _avatarDatabase;
        [SerializeField] private Image _avatar;
        private AsyncOperationHandle _avatarOpHandle;

        public override void Init(HeroSpec heroSpec)
        {
            base.Init(heroSpec);
            var isValid = heroSpec.IsValid();
            StrName.gameObject.SetActive(isValid);
            TxtLevel.gameObject.SetActive(isValid);
            if (isValid == false) return;
            var avatarId = $"{heroSpec.Origin.DetailInformation.Id}-{heroSpec.Class.Id}";
            if (_avatarDatabase.CacheLookupTable.ContainsKey(avatarId) == false) return;
            StartCoroutine(CoLoadAvatar(avatarId));
        }

        private IEnumerator CoLoadAvatar(string avatarId)
        {
            yield return _avatarDatabase.LoadDataByIdAsync(avatarId);
            _avatar.gameObject.SetActive(true);
            _avatarOpHandle = _avatar.LoadSpriteAndSet(_avatarDatabase.CacheLookupTable[avatarId]);
        }

        protected override void Reset()
        {
            base.Reset();
            _avatar.gameObject.SetActive(false);
            MarkAsInteracting(false);
            if (_avatarOpHandle.IsValid()) Addressables.Release(_avatarOpHandle);
        }

        public void MarkAsInteracting(bool isInteracting = true) => _interactingIndicator.SetActive(isInteracting);
    }
}