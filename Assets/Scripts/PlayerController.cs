using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Runtime.Events.ScriptableObjects;
namespace CryptoQuest
{
    public class PlayerController : MonoBehaviour
    {
        public BoxCollider2D facingCollider;
        public Vector2EventChannelSO setPlayerPositionEvent;
        public Vector2EventChannelSO setPlayerFacingAxisEvent;
        private void OnEnable()
        {
            setPlayerPositionEvent.EventRaised += SetPlayerPosition;
            setPlayerFacingAxisEvent.EventRaised += SetPlayerFacingAxis;
        }
        private void OnDisable()
        {
            setPlayerPositionEvent.EventRaised -= SetPlayerPosition;
            setPlayerFacingAxisEvent.EventRaised -= SetPlayerFacingAxis;
        }
        private void SetPlayerPosition(Vector2 position)
        {
            transform.position = position;
        }
        private void SetPlayerFacingAxis(Vector2 axis)
        {
            facingCollider.gameObject.transform.localPosition = axis;
        }
    }
}
