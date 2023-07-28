using System.Collections.Generic;
using CryptoQuest.Audio.AudioData;
using CryptoQuest.Audio.AudioEmitters;
using CryptoQuest.Audio.Settings;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Audio
{
    [RequireComponent((typeof(AudioEmitterPool)))]
    public class AudioManager : MonoBehaviour
    {
        [Header("SoundEmitters Pool")]
        [SerializeField]
        private AudioEmitterPool _pool = default;

        [SerializeField] private int _soundEmitterPoolSize = 10;

        [Header("Listening on")]
        [SerializeField]
        private AudioCueEventChannelSO _sfxEventChannel;

        [SerializeField] private AudioCueEventChannelSO _backgroundMusicEventChannel;

        [Header("Audio control")]
        [SerializeField]
        private AudioSettingsSO _settings;

        private readonly AudioEmitterCache _audioEmitterCache = new();
        private AudioEmitter _playingMusicAudioEmitter;

        private void Awake()
        {
            _pool.Create(_soundEmitterPoolSize);
            _pool.SetParent(this.transform);
        }

        private void OnEnable()
        {
            _sfxEventChannel.AudioPlayRequested += PlayAudioCue;
            _sfxEventChannel.AudioStopRequested += StopAudioCue;

            _backgroundMusicEventChannel.AudioPlayRequested += PlayBackgroundMusic;
            _backgroundMusicEventChannel.AudioStopRequested += StopBackgroundMusic;

            _settings.VolumeChanged += ChangeMasterVolume;
        }

        private void OnDisable()
        {
            _sfxEventChannel.AudioPlayRequested -= PlayAudioCue;
            _sfxEventChannel.AudioStopRequested -= StopAudioCue;

            _backgroundMusicEventChannel.AudioPlayRequested -= PlayBackgroundMusic;
            _backgroundMusicEventChannel.AudioStopRequested -= StopBackgroundMusic;

            _settings.VolumeChanged -= ChangeMasterVolume;

            ReleaseAllSoundEmitters();
        }

        private void ReleaseAllSoundEmitters()
        {
            foreach (var cueToEmitters in _audioEmitterCache.Cache)
            {
                foreach (var soundEmitter in cueToEmitters.Value)
                {
                    soundEmitter.AudioFinishedPlaying -= AudioFinishedPlaying;
                    soundEmitter.Stop();
                    _pool.Release(soundEmitter);
                }
            }
        }

        private void PlayAudioCue(AudioCueSO audioCue)
        {
            AudioClip[] currentClips = audioCue.GetClips();
            List<AudioEmitter> audioEmitters = new();

            var numberOfClips = currentClips.Length;
            for (int i = 0; i < numberOfClips; i++)
            {
                var audioEmitter = _pool.Request();
                if (audioEmitter == null)
                {
                    Debug.LogWarning($"Cannot play audio cue {audioCue} - no sound emitters available");
                    continue;
                }

                audioEmitter.PlayAudioClip(currentClips[i], audioCue.Looping);
                if (!audioCue.Looping) audioEmitter.AudioFinishedPlaying += AudioFinishedPlaying;
                audioEmitters.Add(audioEmitter);
            }

            _audioEmitterCache.Add(audioCue, audioEmitters);
        }


        private void StopAudioCue(AudioCueSO key)
        {
            if (!_audioEmitterCache.TryGetValue(key, out var soundEmitters))
            {
                Debug.LogWarning($"AudioCueKey {key} not found in the SoundEmitterStore");
                return;
            }

            foreach (var soundEmitter in soundEmitters)
            {
                StopAndCleanEmitter(soundEmitter);
            }

            _audioEmitterCache.Remove(key);
        }

        private void PlayBackgroundMusic(AudioCueSO audioCue)
        {
            float fadeDuration = 2f;
            float startTime = 0f;

            if (IsAudioPlaying())
            {
                AudioClip musicToPlay = audioCue.GetClips()[0];
                if (_playingMusicAudioEmitter.GetClip() == musicToPlay) return;
                startTime = _playingMusicAudioEmitter.FadeMusicOut(fadeDuration);
            }

            _playingMusicAudioEmitter = _pool.Request();
            _playingMusicAudioEmitter.FadeMusicIn(audioCue.GetClips()[0], fadeDuration, startTime);
            _playingMusicAudioEmitter.AudioFinishedPlaying += AudioFinishedPlaying;
        }

        private void StopBackgroundMusic(AudioCueSO arg0)
        {
            if (!IsAudioPlaying()) return;

            _playingMusicAudioEmitter.Stop();
        }

        private void ChangeMasterVolume(float value)
        {
            Debug.Log($"Change master volume: {value}");
        }

        private void AudioFinishedPlaying(AudioEmitter audioEmitter)
        {
            StopAndCleanEmitter(audioEmitter);
        }

        private void StopAndCleanEmitter(AudioEmitter audioEmitter)
        {
            audioEmitter.AudioFinishedPlaying -= AudioFinishedPlaying;
            audioEmitter.Stop();
            _pool.Release(audioEmitter);
        }

        private bool IsAudioPlaying() => _playingMusicAudioEmitter != null && _playingMusicAudioEmitter.IsPlaying();
    }
}