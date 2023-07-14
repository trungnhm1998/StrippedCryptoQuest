using CryptoQuest.System.Cutscene.Events;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.System.Cutscene
{
    public class CutsceneSpawner : MonoBehaviour
    {
        [SerializeField] private AssetReference _cutscenePrefab;

        [Header("Listening to")]
        [SerializeField] private CutsceneEventChannelSO _cutsceneRaisedEventChannelSO;

        [Header("Container")]
        [SerializeField] private Transform _spawnPointContainer;

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
            if (_spawnPointContainer == null) return;

            _cutscenePrefab.InstantiateAsync(_spawnPointContainer.position, Quaternion.identity);
        }
    }
}