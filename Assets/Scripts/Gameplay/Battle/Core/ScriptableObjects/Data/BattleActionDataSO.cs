using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "ActionData", menuName = "Gameplay/Battle/Action Data/Battle Action Data")]
    public class BattleActionDataSO : ScriptableObject
    {
        [field: SerializeField]
        public LocalizedString Log { get; private set; }
        [field: SerializeField]
        public AssetReference EffectPrefab { get; private set; }
    }
}
