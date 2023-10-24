using System.Collections;
using UnityEngine;

namespace CryptoQuest.Battle.VFX
{
    public class AutoDestroy : MonoBehaviour
    {
        [SerializeField] private float _delay = 2.1f;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_delay);
            Destroy(gameObject);
        }
    }
}