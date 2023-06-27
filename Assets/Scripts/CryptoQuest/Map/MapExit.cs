using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Map
{
    public class MapExit : MonoBehaviour
    {
        [SerializeField] private SceneScriptableObject _nextScene;
        [SerializeField] private LoadSceneEventChannelSO _loadNextSceneEventChannelSO;
        [SerializeField] private PathStorageSO _transitionSO;
        [SerializeField] private MapPathSO _mapPath;
        [SerializeField] private string _playerTag = "Player";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;

            OnTriggerTeleport(_nextScene);
        }

        private void OnTriggerTeleport(SceneScriptableObject scene)
        {
            UpdateTransitionName(_mapPath);
            _loadNextSceneEventChannelSO.RequestLoad(scene);
        }

        private void UpdateTransitionName(MapPathSO transitionPath)
        {
            _transitionSO.LastTakenPath = transitionPath;
        }
    }
}