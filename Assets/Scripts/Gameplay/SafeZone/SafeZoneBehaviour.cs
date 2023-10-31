using System;
using UnityEngine;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;

namespace CryptoQuest
{
    public class SafeZoneBehaviour : MonoBehaviour
    {
        public static Action OnSafeZoneEntered;
        public static Action OnSafeZoneExited;

        [Header("Area Configuration")] [SerializeField, ReadOnly]
        private string _playerTag = "Player";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            OnSafeZoneEntered?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            OnSafeZoneExited?.Invoke();
        }
    }
}