using CryptoQuest.Map;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Manager
{
    public class EscapeAbilityHandler : MonoBehaviour
    {
        // TODO: Refactor ability
        // [SerializeField] private EscapeAbilitySO _escapeAbilitySo;
        [SerializeField] private SceneScriptableObject _destinationScene;
        [SerializeField] private PathStorageSO _pathStorageSo;
        [SerializeField] private LoadSceneEventChannelSO _loadMapEventChannel;
        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannel;
        [SerializeField] private SpiralConfigSO _spiralConfig;


        private void OnEnable()
        {
            // _escapeAbilitySo.EscapeSucceeded += OnEscapeSucceded;
        }

        private void OnDisable()
        {
            // _escapeAbilitySo.EscapeSucceeded -= OnEscapeSucceded;
        }

        private void OnEscapeSucceded(MapPathSO escapePath)
        {
            _spiralConfig.Color = Color.green;
            _pathStorageSo.LastTakenPath = escapePath;
            _spiralConfig.DoneSpiralIn += TriggerEscape;
            _spiralConfig.DoneSpiralOut += FinishTrasition;
            _onSceneLoadedEventChannel.EventRaised += _spiralConfig.OnSpiralOut;
            _spiralConfig.OnSpiralIn();
        }

        private void TriggerEscape()
        {
            _loadMapEventChannel.RequestLoad(_destinationScene);
        }

        private void FinishTrasition()
        {
            _spiralConfig.DoneSpiralIn -= TriggerEscape;
            _spiralConfig.DoneSpiralOut -= FinishTrasition;
            _onSceneLoadedEventChannel.EventRaised -= _spiralConfig.OnSpiralOut;
        }
    }
}