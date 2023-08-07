using UnityEngine;
using System;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "HeroDataSO", menuName = "Gameplay/Character/Hero Data")]
    public class HeroDataSO : CharacterDataSO
    {
        [field: SerializeField]
        public Sprite BattleIconSprite { get; private set; }
    }
}
