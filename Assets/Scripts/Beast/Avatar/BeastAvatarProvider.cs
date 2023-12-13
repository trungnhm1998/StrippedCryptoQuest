using System.Collections;
using CryptoQuest.UI.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace CryptoQuest.Beast.Avatar
{
    public interface IBeastAvatarProvider
    {
        IEnumerator LoadAvatarAsync(Image avatar, IBeast beast);
    }

    public class BeastAvatarProvider : MonoBehaviour, IBeastAvatarProvider
    {
        [SerializeField] private BeastAvatarSO _avatarDatabase;

        public IEnumerator LoadAvatarAsync(Image avatar, IBeast beast)
        {
            if (!beast.IsValid())
            {
                Debug.LogWarning("UIBeastDetail::SetAvatar::Invalid Beast");
                yield break;
            }

            AssetReferenceT<Sprite> assetRefAvatar = FindAvatarMapping(beast);

            if (assetRefAvatar == null || !assetRefAvatar.RuntimeKeyIsValid())
            {
                avatar.enabled = false;
                yield break;
            }

            yield return null;
            avatar.LoadSpriteAndSet(assetRefAvatar);
        }

        private AssetReferenceT<Sprite> FindAvatarMapping(IBeast beast)
        {
            foreach (var avatar in _avatarDatabase.AvatarMappings)
            {
                if (IsMatchingAvatar(avatar, beast))
                {
                    return avatar.Image;
                }
            }

            return null;
        }

        private bool IsMatchingAvatar(BeastAvatarData avatar, IBeast beast)
        {
            return avatar.BeastId == beast.Type.Id &&
                   avatar.ElementId == beast.Elemental.Id &&
                   avatar.ClassId == beast.Class.Id;
        }
    }
}