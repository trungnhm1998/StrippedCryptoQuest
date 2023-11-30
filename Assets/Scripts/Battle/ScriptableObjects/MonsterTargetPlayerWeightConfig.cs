using UnityEngine;

namespace CryptoQuest.Battle.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MonsterTargetPlayerWeightConfig",
        menuName = "Gameplay/Battle/Data/Monster Target Player Weight Config")]
    public class MonsterTargetPlayerWeightConfig : ScriptableObject 
    {
        [Tooltip("Weight to decide which hero will monster target. Lenght should be more than hero slot.")]
        [field: SerializeField]
        public int[] Weights { get; private set; }
    }
}