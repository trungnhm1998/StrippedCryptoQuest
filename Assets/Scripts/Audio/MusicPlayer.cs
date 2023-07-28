using System;
using CryptoQuest.Audio.AudioData;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _onSceneReady;
        [SerializeField] private AudioCueEventChannelSO _onPlayMusic;
        [SerializeField] private AudioCueSO _musicTrack;
        [SerializeField] private AudioConfigurationSO _audioConfig;

        private void OnEnable()
        {
            _onSceneReady.EventRaised += PlayMusic;
        }

        private void OnDisable()
        {
            _onSceneReady.EventRaised -= PlayMusic;
        }

        public void PlayMusic()
        {
            _onPlayMusic.RaisePlayEvent(_musicTrack, _audioConfig);
        }
    }
}