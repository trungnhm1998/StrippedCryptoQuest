using CryptoQuest.Audio.AudioData;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Audio
{
    public class PlayMusicOnSceneLoaded : MonoBehaviour
    {
        [Header("Listen to")] [SerializeField] private VoidEventChannelSO _sceneLoaded;
        [FormerlySerializedAs("_playMusicEvent")] [Header("Raise on")] [SerializeField] private AudioCueEventChannelSO _musicEventChannel;

        [Header("Configs")] [SerializeField] private AudioCueSO _musicTrack;

        private void OnEnable()
        {
            _sceneLoaded.EventRaised += OnPlayMusic;
        }

        private void OnDisable()
        {
            _sceneLoaded.EventRaised -= OnPlayMusic;
        }

        private void OnPlayMusic()
        {
            _musicEventChannel.PlayAudio(_musicTrack);
        }
    }
}