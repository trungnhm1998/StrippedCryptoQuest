using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.GameObjectPosition
{
    public class GameObjectPositionReceiver : MonoBehaviour, INotificationReceiver
    {
        public void OnNotify(Playable origin, INotification notification, object context)
        {
            if (notification is not GameObjectPositionMarker marker) return;
            Debug.Log($"GameObjectPositionNotificationReceiver:[{name}] placed at: {marker.Position}");
            transform.position = marker.Position;
        }
    }
}