using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Map
{
    public class MapExit : MonoBehaviour
    {
        public SceneScriptableObject nextScene;
        public LoadSceneEventChannelSO loadNextSceneEventChannelSO;
        public MapTransitionSO transitionSO;
        public MapPathSO mapPath;
        [SerializeField] private string _playerTag = "Player";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;

            OnTriggerTeleport(nextScene);
        }

        private void OnTriggerTeleport(SceneScriptableObject scene)
        {
            UpdateTransitionName(mapPath);
            loadNextSceneEventChannelSO.RequestLoad(scene);
        }
        private void UpdateTransitionName(MapPathSO transitionPath)
        {
            transitionSO.currentMapPath = transitionPath;
        }
    }
}