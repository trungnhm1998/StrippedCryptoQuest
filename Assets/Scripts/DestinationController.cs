using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Runtime.Events.ScriptableObjects;

namespace CryptoQuest
{
    public class DestinationController : MonoBehaviour
    {
        public PlayerPositionInfoSO positionInfoSO;
        public Vector2EventChannelSO setPlayerPositionEvent;
        public Vector2EventChannelSO setPlayerFacingAxisEvent;

        private void Start()
        {
            setPlayerPositionEvent.RaiseEvent(transform.position);
            setPlayerFacingAxisEvent.RaiseEvent(positionInfoSO.playerFacingAxis);
        }
    }
}
