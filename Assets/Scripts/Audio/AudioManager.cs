using CryptoQuest.Audio.AudioData;
using CryptoQuest.Audio.AudioEmitters;
using CryptoQuest.Audio.Settings;
using UnityEngine;

namespace CryptoQuest.Audio
{
    [RequireComponent((typeof(AudioEmitterPool)))]
    public class AudioManager : MonoBehaviour
    {
        [Header("SoundEmitters Pool")] [SerializeField]
        private AudioEmitterPool _pool = default;

        [SerializeField] private int _soundEmitterPoolSize = 10;

        [Header("Listening on")] [SerializeField]
        private AudioCueEventChannelSO _sfxEventChannel;

        [SerializeField] private AudioCueEventChannelSO _backgroundMusicEventChannel;

        [Header("Audio control")] [SerializeField]
        private AudioSettingsSO _settings;

        private AudioEmitter _playingMusicAudioEmitter;

        private void Awake()
        {
            _pool.Create(_soundEmitterPoolSize);
            _pool.SetParent(this.transform);
        }

        private void OnEnable()
        {
            _sfxEventChannel.AudioPlayRequested += PlaySFX;
            _sfxEventChannel.AudioStopRequested += StopSFX;

            _backgroundMusicEventChannel.AudioPlayRequested += PlayBackgroundMusic;
            _backgroundMusicEventChannel.AudioStopRequested += StopBackgroundMusic;

            _settings.VolumeChanged += ChangeMasterVolume;
        }

        private void OnDisable()
        {
            _sfxEventChannel.AudioPlayRequested -= PlaySFX;
            _sfxEventChannel.AudioStopRequested -= StopSFX;

            _backgroundMusicEventChannel.AudioPlayRequested -= PlayBackgroundMusic;
            _backgroundMusicEventChannel.AudioStopRequested -= StopBackgroundMusic;

            _settings.VolumeChanged -= ChangeMasterVolume;
        }

        private void PlaySFX(AudioCueSO audioCue)
        {
            AudioClip[] currentClips = audioCue.GetClips();

            var numberOfClips = currentClips.Length;
            for (int i = 0; i < numberOfClips; i++)
            {
                var audioEmitter = _pool.Request();
                if (audioEmitter == null)
                {
                    Debug.LogWarning(
                        $"Cannot play audio cue [{audioCue}] with clip [{currentClips[i].name}] " +
                        $"- no sound emitters available");
                    continue;
                }

                audioEmitter.PlayAudioClip(currentClips[i], _settings, audioCue.Looping);
                if (!audioCue.Looping) audioEmitter.AudioFinishedPlaying += AudioFinishedPlaying;
            }
        }

        /// <summary>
        /// All SFX are one shot, so we can release the emitter back to the pool
        /// Let the pool destroy it if it's not needed anymore
        /// ┐(´～｀)┌
        /// </summary>
        /// <param name="key"></param>
        private void StopSFX(AudioCueSO key) { }

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

            if (_playingMusicAudioEmitter == null)
                _playingMusicAudioEmitter = _pool.Request();
            _playingMusicAudioEmitter.FadeMusicIn(audioCue.GetClips()[0], _settings, fadeDuration, startTime);
        }

        private void StopBackgroundMusic(AudioCueSO arg0)
        {
            if (!IsAudioPlaying()) return;

            _playingMusicAudioEmitter.Stop();
        }

        private void ChangeMasterVolume(float value)
        {
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
        }

        private bool IsAudioPlaying() => _playingMusicAudioEmitter != null && _playingMusicAudioEmitter.IsPlaying();
    }
}