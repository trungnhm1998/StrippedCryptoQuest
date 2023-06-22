using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using System;
using Core.Runtime.Common;
using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;

namespace CryptoQuest
{
    public class TeleportArea : MonoBehaviour
    {
        public SceneScriptableObject currentScene;
        public SceneScriptableObject nextScene;
        public PlayerPositionInfoSO playerPositionInfoSO;
        public LoadSceneEventChannelSO _loadNextSceneEventChannelSO;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController player = other.gameObject.GetComponent<PlayerController>();
                UpdatePlayerFacingAxis(player.facingCollider.transform.position - player.transform.position);
                TriggerTeleport(nextScene);
            }
        }
        private void TriggerTeleport(SceneScriptableObject scene)
        {
            _loadNextSceneEventChannelSO.RequestLoad(scene);
        }

        private void UpdatePlayerFacingAxis(Vector2 axis)
        {
            playerPositionInfoSO.playerFacingAxis = axis;
        }
    }
}
