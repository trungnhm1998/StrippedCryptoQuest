using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Battle.VFX
{
    [CreateAssetMenu(fileName = "New VFX", menuName = "Crypto Quest/Battle/VFX/New Move VFX")]
    public class VFXMoveDataSO : VFXDataSO
    {
        [SerializeField] private Vector3 _direction;
        public override IEnumerator Execute(Transform parent)
        {
            var go = Instantiate(VfxPrefab, parent);
            Destroy(go, _effectLenght);
            while(go != null)
            {
                go.transform.position += _direction;
                yield return null;
            }    
        }
    }
}
