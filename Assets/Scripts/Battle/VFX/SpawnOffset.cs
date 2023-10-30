using UnityEngine;

namespace CryptoQuest.Battle.VFX
{
    public class SpawnOffset : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset = new Vector3(0, 0.2f, 0);

        private void Awake() => transform.position += _offset;
    }
}