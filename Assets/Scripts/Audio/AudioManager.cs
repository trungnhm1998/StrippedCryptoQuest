using CryptoQuest.Audio.AudioData;
using CryptoQuest.Audio.AudioEmitters;
using CryptoQuest.Audio.Settings;
using CryptoQuest.Core;
using System;
using CryptoQuest.SaveSystem.Sagas;
using UnityEngine;

namespace CryptoQuest.Audio
{
    [RequireComponent((typeof(AudioEmitterPool)))]
    public class AudioManager : MonoBehaviour
    {
        [Header("AudioEmitters Pool")]
        [SerializeField]
        private AudioEmitterPool _pool = default;

        [SerializeField] private int _audioEmitterPoolSize = 10;

        [Header("Listening on")]
        [SerializeField]
        private AudioCueEventChannelSO _sfxEventChannel;

        [SerializeField] private AudioCueEventChannelSO _backgroundMusicEventChannel;

        [Header("Audio control"), SerializeField]
        private AudioSettingSO audioSettings;

        private AudioEmitter _playingMusicAudioEmitter;
        private AudioCueSO _currentSfxCue;
        private AudioCueSO _currentBgmCue;

        private void Awake()
        {
            _pool.Create(_audioEmitterPoolSize);
            _pool.SetParent(this.transform);
        }

        protected void OnEnable()
        {
            _sfxEventChannel.AudioPlayRequested += PlaySfx;
            _sfxEventChannel.AudioStopRequested += StopSfx;

            _backgroundMusicEventChannel.AudioPlayRequested += PlayBackgroundMusic;
            _backgroundMusicEventChannel.AudioStopRequested += StopBackgroundMusic;

            audioSettings.VolumeChanged += ChangeMasterVolume;
        }

        protected void OnDisable()
        {
            _sfxEventChannel.AudioPlayRequested -= PlaySfx;
            _sfxEventChannel.AudioStopRequested -= StopSfx;

            _backgroundMusicEventChannel.AudioPlayRequested -= PlayBackgroundMusic;
            _backgroundMusicEventChannel.AudioStopRequested -= StopBackgroundMusic;

            audioSettings.VolumeChanged -= ChangeMasterVolume;
        }

        public void PlaySfx(AudioCueSO audioCue)
        {
            TryToLoadData(audioCue, currentClip =>
            {
                var audioEmitter = _pool.Request();
                if (audioEmitter == null)
                {
                    Debug.LogWarning(
                        $"Cannot play audio cue [{audioCue}] " +
                        $"- no sound emitters available");
                    return;
                }

                audioEmitter.PlayAudioClip(currentClip, audioSettings, audioCue.Looping);
                if (!audioCue.Looping) audioEmitter.AudioFinishedPlaying += AudioFinishedPlaying;

                _currentSfxCue = audioCue;
                Debug.Log($"[AudioManager::PlaySFX] Playing SFX: {audioCue.name}");
            });
        }


        /// <summary>
        /// All SFX are one shot, so we can release the emitter back to the pool
        /// Let the pool destroy it if it's not needed anymore
        /// ┐(´～｀)┌
        /// </summary>
        /// <param name="key"></param>
        private void StopSfx(AudioCueSO key) { }

        public void PlayBackgroundMusic(AudioCueSO audioCue)
        {
            float startTime = 0f;

            if (_currentBgmCue != null)
            {
                _currentBgmCue.GetPlayableAsset().ReleaseAsset();
            }

            TryToLoadData(audioCue, currentClip =>
            {
                if (IsAudioPlaying())
                {
                    AudioClip musicToPlay = currentClip;
                    if (_playingMusicAudioEmitter.GetClip() == musicToPlay) return;
                    startTime = _playingMusicAudioEmitter.FadeMusicOut();
                }

                if (_playingMusicAudioEmitter == null)
                    _playingMusicAudioEmitter = _pool.Request();
                _playingMusicAudioEmitter.FadeMusicIn(currentClip, audioSettings, startTime);

                _currentBgmCue = audioCue;

                Debug.Log($"[AudioManager::PlayBackgroundMusic] Playing background music: {audioCue.name}");
            });
        }

        private void TryToLoadData(AudioCueSO audioCue, Action<AudioClip> callback)
        {
            var currentCue = audioCue.GetPlayableAsset();

            if (currentCue.IsValid())
            {
                if (currentCue.Asset != null)
                {
                    callback?.Invoke((AudioClip)currentCue.Asset);
                    return;
                }

                currentCue.ReleaseAsset();
            }

            currentCue.LoadAssetAsync().Completed += handle => { callback?.Invoke(handle.Result); };
        }


        private void StopBackgroundMusic(AudioCueSO arg0)
        {
            if (!IsAudioPlaying()) return;

            _playingMusicAudioEmitter.Stop();
            Debug.Log($"[AudioManager] Stopped playing background music");
        }

        private void ChangeMasterVolume(float value)
        {
            if (!IsAudioPlaying()) return;
            _playingMusicAudioEmitter.SetVolume(value);
        }

        private void AudioFinishedPlaying(AudioEmitterValue audioEmitterValue)
        {
            StopAndCleanEmitter(audioEmitterValue);
        }

        private void StopAndCleanEmitter(AudioEmitterValue audioEmitterValue)
        {
            audioEmitterValue.UnregisterEvent(AudioFinishedPlaying);
            audioEmitterValue.Stop();
            audioEmitterValue.ReleaseToPool();

            if (_currentSfxCue != null)
            {
                _currentSfxCue.GetPlayableAsset().ReleaseAsset();
            }
        }

        private bool IsAudioPlaying() => _playingMusicAudioEmitter != null && _playingMusicAudioEmitter.IsPlaying();
    }
}