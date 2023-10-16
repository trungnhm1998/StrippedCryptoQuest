using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Battle.VFX
{
    [CreateAssetMenu(fileName = "New VFX", menuName = "Crypto Quest/Battle/VFX/New VFX")]
    public class VFXDataSO : ScriptableObject
    {
        public string Id;
        public GameObject VfxPrefab;
        [SerializeField] protected float _effectLenght = 2.1f;
        [SerializeField] private int _sortingOder = 1000;
        private const string SORTING_LAYER = "Battle";

        public void Init()
        {
            ParticleSystemRenderer[] childs = VfxPrefab.GetComponentsInChildren<ParticleSystemRenderer>();
            foreach (ParticleSystemRenderer psr in childs)
            {
                psr.sortingOrder = _sortingOder;
                psr.sortingLayerName = SORTING_LAYER;
            }
        }

        public virtual IEnumerator Execute(Transform parent)
        {
            var go = Instantiate(VfxPrefab, parent);
            Destroy(go, _effectLenght);
            yield return new WaitUntil(() => go == null);
        }
    }
}
