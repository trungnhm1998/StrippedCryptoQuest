using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.GameObjectPosition
{
    [CustomStyle("GameObjectPositionMarkerStyle")]
    [DisplayName("GameObjectPosition")]
    public class GameObjectPositionMarker : Marker, INotification
    {
        public PropertyName id => new();

        public Vector3 Position;
        public Color Color;
    }
}