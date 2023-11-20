using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Character.Avatar
{
    [CreateAssetMenu(fileName = "HeroAvatarSO", menuName = "Gameplay/Character/Avatar")]
    public class HeroAvatarSetSO : ScriptableObject
    {
        [SerializeField] private HeroAvatarSet[] _avatarMappings = Array.Empty<HeroAvatarSet>();
        public HeroAvatarSet[] AvatarMappings => _avatarMappings;
    }
}
