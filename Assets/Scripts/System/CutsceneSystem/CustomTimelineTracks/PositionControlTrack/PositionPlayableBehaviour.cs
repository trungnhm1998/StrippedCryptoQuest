using System;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.Timeline.Position
{
    [Serializable]
    public class PositionPlayableBehaviour : PlayableBehaviour
    {
        [HideInInspector]
        public Vector3 Position;
    }
}