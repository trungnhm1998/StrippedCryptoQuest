using CryptoQuest.Quest.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.SafeZone
{
    public class SafeZoneCollider : MonoBehaviour
    {
        [SerializeField] private TriggerActionCollider _triggerActionCollider;
        [SerializeField] private BoxCollider2D _safeZoneCollider;
        [SerializeField] private float _paddingSize = 5f;

        private void OnEnable()
        {
            _triggerActionCollider.OnSizeChanged += OnSizeChanged;
        }

        private void OnDisable()
        {
            _triggerActionCollider.OnSizeChanged -= OnSizeChanged;
        }

        private void OnSizeChanged(Vector2 size)
        {
            _safeZoneCollider.size = new Vector2(size.x + _paddingSize, size.y + _paddingSize);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            SafeZoneController.OnSafeZoneEntered?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            SafeZoneController.OnSafeZoneExited?.Invoke();
        }
    }
}