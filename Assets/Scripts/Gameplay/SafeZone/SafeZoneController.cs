using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;

namespace CryptoQuest
{
    public class SafeZoneController : MonoBehaviour
    {
        [ReadOnly] public static bool IsSafeZoneActive = false;

        private void OnEnable()
        {
            SafeZoneBehaviour.OnSafeZoneEntered += EnterSafeZone;
            SafeZoneBehaviour.OnSafeZoneExited += ExitSafeZone;
        }

        private void OnDisable()
        {
            SafeZoneBehaviour.OnSafeZoneEntered -= EnterSafeZone;
            SafeZoneBehaviour.OnSafeZoneExited -= ExitSafeZone;
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