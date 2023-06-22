using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest
{
    public class DestinationController : MonoBehaviour
    {
        public GameObject playerPrefab;
        public PlayerPositionInfoSO positionInfo;
        private PlayerController player;

        private void Start()
        {
            player = Instantiate(playerPrefab, positionInfo.playerPosition, Quaternion.identity).GetComponent<PlayerController>();
            player.facingCollider.transform.position = positionInfo.playerFacingAxis;
        }
    }
}
