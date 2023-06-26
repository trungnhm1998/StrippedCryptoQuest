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
using CryptoQuest.Characters;

namespace CryptoQuest
{
    public class TeleportArea : MonoBehaviour
    {
        [SerializeField] private String _playerTag = "Player";
        public SceneScriptableObject nextScene;
        public LoadSceneEventChannelSO _loadNextSceneEventChannelSO;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(_playerTag))
            {
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                playerController.SaveFacingDirection(playerController.GetFacingDirection());
                OnTriggerTeleport(nextScene);
            }
        }
        private void OnTriggerTeleport(SceneScriptableObject scene)
        {
            _loadNextSceneEventChannelSO.RequestLoad(scene);
        }
    }
}
