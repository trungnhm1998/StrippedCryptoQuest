using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Character.MonoBehaviours
{
    public class ZoneTriggerController : MonoBehaviour
    {
        [SerializeField] private UnityEvent<bool, GameObject> _enterZone = default;
        [SerializeField] private LayerMask _layers = default;
        public static readonly string Tag = "NPC";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((1 << other.gameObject.layer & _layers) != 0 && other.tag == Tag)
            {
                _enterZone.Invoke(true, other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if ((1 << other.gameObject.layer & _layers) != 0 && other.tag == Tag)
            {
                _enterZone.Invoke(false, other.gameObject);
            }
        }
    }
}