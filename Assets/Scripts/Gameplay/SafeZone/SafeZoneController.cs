using System;
using UnityEngine;

namespace CryptoQuest
{
    public class SafeZoneController : MonoBehaviour
    {
        public static bool IsSafeZoneActive = false;
        public static Action OnSafeZoneEntered;
        public static Action OnSafeZoneExited;

        private void OnEnable()
        {
            OnSafeZoneEntered += EnterSafeZone;
            OnSafeZoneExited += ExitSafeZone;
        }

        private void OnDisable()
        {
            OnSafeZoneEntered -= EnterSafeZone;
            OnSafeZoneExited -= ExitSafeZone;
        }


        private void EnterSafeZone()
        {
            IsSafeZoneActive = true;
        }

        private void ExitSafeZone()
        {
            IsSafeZoneActive = false;
        }
    }
}