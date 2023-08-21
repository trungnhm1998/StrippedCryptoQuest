using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "BattleBackgroundSO", menuName = "Gameplay/Battle/Battle Background")]
    public class BattleBackgroundSO : ScriptableObject
    {
        public int Id;
        public AssetReferenceT<Sprite> BattleBackground;
    }
}