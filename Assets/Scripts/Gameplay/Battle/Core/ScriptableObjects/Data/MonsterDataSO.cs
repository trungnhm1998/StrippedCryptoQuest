using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "MonsterDataSO", menuName = "Gameplay/Character/Monster Data")]
    public class MonsterDataSO : CharacterDataSO
    {
        [field: SerializeField]
        public AssetReference MonsterPrefab { get; private set; }
    }
}
