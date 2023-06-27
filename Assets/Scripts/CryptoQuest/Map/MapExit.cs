using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Map
{
    public class MapExit : MonoBehaviour
    {
        public SceneScriptableObject nextScene;
        public LoadSceneEventChannelSO loadNextSceneEventChannelSO;

        [SerializeField] private string _playerTag = "Player";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;

            OnTriggerTeleport(nextScene);
        }

        private void OnTriggerTeleport(SceneScriptableObject scene)
        {
            loadNextSceneEventChannelSO.RequestLoad(scene);
        }
    }
}