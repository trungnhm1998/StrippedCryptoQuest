using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.System.CutScene
{
    public class CutsceneSpawner : MonoBehaviour
    {
        [SerializeField] private AssetReference _cutscenePrefab;

        [Header("Listening to")]
        [SerializeField] private VoidEventChannelSO _cutsceneRaisedEventChannelSO;

        private Transform _defaultSpawnPoint;

        private void Awake()
        {
            _defaultSpawnPoint = transform.GetChild(0);
        }

        private void OnEnable()
        {
            _cutsceneRaisedEventChannelSO.EventRaised += SpawnCutscene;
        }

        private void OnDisable()
        {
            _cutsceneRaisedEventChannelSO.EventRaised -= SpawnCutscene;
        }

        private void SpawnCutscene()
        {
            if (_defaultSpawnPoint == null) return;

            _cutscenePrefab.InstantiateAsync(_defaultSpawnPoint.position, Quaternion.identity);
        }
    }
}