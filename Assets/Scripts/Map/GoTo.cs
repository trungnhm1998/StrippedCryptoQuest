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

        [SerializeField] private BoxCollider2D _collider;
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

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 255, 0, .3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(new Vector3(_collider.offset.x, _collider.offset.y, -2), _collider.size);
        }

        public void SetUpTeleportInfo(SceneScriptableObject nextScene, MapPathSO mapPath)
        {
            _nextScene = nextScene;
            _mapPath = mapPath;
        }
    }
}