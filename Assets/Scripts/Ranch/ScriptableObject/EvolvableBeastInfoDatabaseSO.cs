using CryptoQuest.Ranch.Evolve;
using UnityEngine;

namespace CryptoQuest.Ranch.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ranch/Evolve/EvolvableModel")]
    public class EvolvableBeastInfoDatabaseSO : UnityEngine.ScriptableObject
    {
        [field: SerializeField] public EvolvableBeastInfoDatabase[] EvolableInfos { get; private set; }
    }
}