using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Map
{
    public class GoTo : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private SceneScriptableObject _nextScene;

        [SerializeField] private MapPathSO _mapPath;
        public MapPathSO MapPath => _mapPath;

        [Header("Refs")]
        [SerializeField] private LoadSceneEventChannelSO _loadNextSceneEventChannelSO;

        [SerializeField] private PathStorageSO _transitionSO;
        [SerializeField, ReadOnly] private string _playerTag = "Player";

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