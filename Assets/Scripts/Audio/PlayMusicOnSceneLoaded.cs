using CryptoQuest.Audio.AudioData;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Audio
{
    public class PlayMusicOnSceneLoaded : MonoBehaviour
    {
        [Header("Listen to")] [SerializeField] private VoidEventChannelSO _sceneLoaded;
        [Header("Raise on")] [SerializeField] private AudioCueEventChannelSO _musicEventChannel;

        [Header("Configs")] public AudioCueSO MusicTrack;

        private void OnEnable()
        {
            _sceneLoaded.EventRaised += PlayBackgroundMusic;
        }

        private void OnDisable()
        {
            _sceneLoaded.EventRaised -= PlayBackgroundMusic;
        }

        public void PlayBackgroundMusic()
        {
            _musicEventChannel.PlayAudio(MusicTrack);
        }

        public void StopBackgroundMusic()
        {
            _musicEventChannel.StopAudio(MusicTrack);
        }
    }
}