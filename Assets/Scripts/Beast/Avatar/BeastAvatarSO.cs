using System;
using UnityEngine;

namespace CryptoQuest.Beast.Avatar
{
    [CreateAssetMenu(fileName = "BeastAvatarSO", menuName = "Gameplay/Character/Avatar/Beast")]
    public class BeastAvatarSO : ScriptableObject
    {
        [SerializeField] private BeastAvatarData[] _avatarMappings = Array.Empty<BeastAvatarData>();
        public BeastAvatarData[] AvatarMappings => _avatarMappings;
    }
}