using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Character.MonoBehaviours
{
    public class ZoneTriggerController : MonoBehaviour
    {
        [SerializeField] private UnityEvent<bool, GameObject> _enterZone = default;
        [SerializeField] private LayerMask _layers;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((1 << other.gameObject.layer & _layers) != 0)
            {
                _enterZone.Invoke(true, other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if ((1 << other.gameObject.layer & _layers) != 0)
            {
                _enterZone.Invoke(false, other.gameObject);
            }
        }
    }
}