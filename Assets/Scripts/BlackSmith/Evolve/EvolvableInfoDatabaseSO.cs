using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve
{
    [CreateAssetMenu(menuName = "Crypto Quest/BlackSmith/Evolve/EvolvableModel")]
    public class EvolvableInfoDatabaseSO : ScriptableObject
    {
        [field: SerializeField] public EvolvableInfoData[] EvolableInfos { get; private set; }

#if UNITY_EDITOR
        public void Editor_SetEvolableInfos(EvolvableInfoData[] evolableInfos) => EvolableInfos = evolableInfos;
#endif
    }
}