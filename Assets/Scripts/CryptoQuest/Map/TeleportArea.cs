using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Map
{
    public class TeleportArea : MonoBehaviour
    {
        public SceneScriptableObject NextScene;
        public LoadSceneEventChannelSO LoadNextSceneEventChannelSO;

        [SerializeField] private string _playerTag = "Player";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;

            OnTriggerTeleport(NextScene);
        }

        private void OnTriggerTeleport(SceneScriptableObject scene)
        {
            LoadNextSceneEventChannelSO.RequestLoad(scene);
        }
    }
}