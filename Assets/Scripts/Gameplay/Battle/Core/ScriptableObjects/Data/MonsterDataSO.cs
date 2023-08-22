using CryptoQuest.Gameplay.BaseGameplayData;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "MonsterDataSO", menuName = "Gameplay/Character/Monster Data")]
    public class MonsterDataSO : CharacterDataSO
    {
        public int MonsterId;
        public Element Element;
        public float Exp;
        public float Gold;
        public string DropItemID;

        [field: SerializeField]
        public AssetReference MonsterPrefab { get; private set; }
#if UNITY_EDITOR
        public void Editor_SetMonsterPrefab(AssetReference prefab)
        {
            MonsterPrefab = prefab;
        }
#endif
    }
}