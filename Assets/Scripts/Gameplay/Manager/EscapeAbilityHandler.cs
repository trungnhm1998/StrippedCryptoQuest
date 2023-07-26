using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using CryptoQuest.Map;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Manager
{
    public class EscapeAbilityHandler : MonoBehaviour
    {
        [SerializeField] private EscapeAbilitySO _escapeAbilitySo;
        [SerializeField] private SceneScriptableObject _destinationScene;
        [SerializeField] private PathStorageSO _pathStorageSo;
        [SerializeField] private LoadSceneEventChannelSO _loadMapEventChannel;
        [SerializeField] private SpiralConfigSO _spiralConfig;


        private void OnEnable()
        {
            _escapeAbilitySo.EscapeSucceeded += OnEscapeSucceded;
        }

        private void OnDisable()
        {
            _escapeAbilitySo.EscapeSucceeded -= OnEscapeSucceded;
        }

        private void OnEscapeSucceded(MapPathSO escapePath)
        {
            _spiralConfig.Color = Color.green;
            _pathStorageSo.LastTakenPath = escapePath;
            _loadMapEventChannel.RequestLoad(_destinationScene);
        }
    }
}