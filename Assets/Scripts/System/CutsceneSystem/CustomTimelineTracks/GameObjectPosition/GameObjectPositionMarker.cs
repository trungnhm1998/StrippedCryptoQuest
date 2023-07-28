using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.GameObjectPosition
{
    [CustomStyle("GameObjectPositionMarkerStyle")]
    [DisplayName("GameObjectPosition1")]
    public class GameObjectPositionMarker : Marker, INotification
    {
        public PropertyName id => new();

        public Vector3 Position = Vector3.zero;
#if UNITY_EDITOR
        public string Description = "Position";
        public Color Color = Color.white;
#endif
    }
}