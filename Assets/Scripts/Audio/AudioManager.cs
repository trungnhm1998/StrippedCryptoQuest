using CryptoQuest.Audio.AudioData;
using CryptoQuest.Audio.SoundEmitters;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Audio
{
    [RequireComponent((typeof(SoundEmitterPool)))]
    public class AudioManager : MonoBehaviour
    {
        [Header("SoundEmitters Pool")]
        [SerializeField] private SoundEmitterPool _pool = default;

        [SerializeField] private int _soundEmitterPoolSize = 10;

        [Header("Listening on")]
        [SerializeField] private AudioCueEventChannelSO _sfxEventChannel;

        [SerializeField] private AudioCueEventChannelSO _backgroundMusicEventChannel;
        [SerializeField] private FloatEventChannelSO _sfxVolumeEventChannel;
        [SerializeField] private FloatEventChannelSO _backgroundMusicVolumeEventChannel;
        [SerializeField] private FloatEventChannelSO _masterVolumeEventChannel;

        [Header("Audio control")]
        [Range(0f, 1f)]
        [SerializeField] private float _masterVolume = 1f;

        [Range(0f, 1f)]
        [SerializeField] private float _backgroundMusicVolume = 1f;

        [Range(0f, 1f)]
        [SerializeField] private float _sfxVolume = 1f;

        private SoundEmitterStore _soundEmitterStore;
        private SoundEmitter _musicSoundEmitter;


        private void OnValidate()
        {
            _pool = GetComponent<SoundEmitterPool>();
        }

        private void Awake()
        {
            _soundEmitterStore = new SoundEmitterStore();

            _pool.Create(_soundEmitterPoolSize);
            _pool.SetParent(this.transform);
        }

        private void OnEnable()
        {
            _sfxEventChannel.OnAudioCuePlayRequested += PlayAudioCue;
            _sfxEventChannel.OnAudioCueStopRequested += StopAudioCue;
            _sfxEventChannel.OnAudioCueFinishRequested += FinishAudioCue;

            _backgroundMusicEventChannel.OnAudioCuePlayRequested += PlayBackgroundMusic;
            _backgroundMusicEventChannel.OnAudioCueStopRequested += StopBackgroundMusic;

            _masterVolumeEventChannel.EventRaised += ChangeMasterVolume;
            _backgroundMusicVolumeEventChannel.EventRaised += ChangeBackgroundMusicVolume;
            _sfxVolumeEventChannel.EventRaised += ChangeSfxVolume;
        }

        private void OnDisable()
        {
            _sfxEventChannel.OnAudioCuePlayRequested -= PlayAudioCue;
            _sfxEventChannel.OnAudioCueStopRequested -= StopAudioCue;
            _sfxEventChannel.OnAudioCueFinishRequested -= FinishAudioCue;

            _backgroundMusicEventChannel.OnAudioCuePlayRequested -= PlayBackgroundMusic;
            _backgroundMusicEventChannel.OnAudioCueStopRequested -= StopBackgroundMusic;

            _masterVolumeEventChannel.EventRaised -= ChangeMasterVolume;
            _backgroundMusicVolumeEventChannel.EventRaised -= ChangeBackgroundMusicVolume;
            _sfxVolumeEventChannel.EventRaised -= ChangeSfxVolume;
        }

        private AudioCueKey PlayAudioCue(AudioCueSO audioCue, AudioConfigurationSO settings)
        {
            AudioClip[] currentClips = audioCue.GetClips();
            SoundEmitter[] soundEmitters = new SoundEmitter[currentClips.Length];

            var clipIndex = currentClips.Length;
            for (int i = 0; i < clipIndex; i++)
            {
                soundEmitters[i] = _pool.Request();
                if (soundEmitters[i] == null) continue;

                soundEmitters[i].PlayAudioClip(currentClips[i], settings, audioCue.looping);
                if (!audioCue.looping) soundEmitters[i].OnSoundFinishedPlaying += OnSoundFinishedPlaying;
            }

            return _soundEmitterStore.Add(audioCue, soundEmitters);
        }


        private bool StopAudioCue(AudioCueKey key)
        {
            bool isFound = _soundEmitterStore.Get(key, out SoundEmitter[] soundEmitters);

            if (!isFound)
            {
                Debug.LogWarning($"AudioCueKey {key} not found in the SoundEmitterStore");
                return false;
            }

            foreach (var soundEmitter in soundEmitters)
            {
                StopAndCleanEmitter(soundEmitter);
            }

            _soundEmitterStore.Remove(key);

            return true;
        }

        private bool FinishAudioCue(AudioCueKey key)
        {
            bool isFound = _soundEmitterStore.Get(key, out SoundEmitter[] soundEmitters);

            if (!isFound)
            {
                Debug.LogWarning($"AudioCueKey {key} not found in the SoundEmitterStore");
                return false;
            }

            foreach (var soundEmitter in soundEmitters)
            {
                soundEmitter.Finish();
                soundEmitter.OnSoundFinishedPlaying += OnSoundFinishedPlaying;
            }

            return true;
        }

        private AudioCueKey PlayBackgroundMusic(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration)
        {
            float fadeDuration = 2f;
            float startTime = 0f;

            if (IsAudioPlaying())
            {
                AudioClip musicToPlay = audioCue.GetClips()[0];
                if (_musicSoundEmitter.GetClip() == musicToPlay) return AudioCueKey.Invalid;
                startTime = _musicSoundEmitter.FadeMusicOut(fadeDuration);
            }

            _musicSoundEmitter = _pool.Request();
            _musicSoundEmitter.FadeMusicIn(audioCue.GetClips()[0], audioConfiguration, fadeDuration, startTime);
            _musicSoundEmitter.OnSoundFinishedPlaying += OnSoundFinishedPlaying;

            return AudioCueKey.Invalid;
        }

        private bool StopBackgroundMusic(AudioCueKey key)
        {
            if (!IsAudioPlaying()) return false;

            _musicSoundEmitter.Stop();
            return true;
        }

        private void ChangeMasterVolume(float value)
        {
            Debug.Log($"Change master volume: {value}");
        }

        private void ChangeBackgroundMusicVolume(float value)
        {
            Debug.Log($"Change bg music volume: {value}");
        }

        private void ChangeSfxVolume(float value)
        {
            Debug.Log($"Change sfx  volume: {value}");
        }

        private void OnSoundFinishedPlaying(SoundEmitter soundEmitter)
        {
            StopAndCleanEmitter(soundEmitter);
        }

        private void StopAndCleanEmitter(SoundEmitter soundEmitter)
        {
            if (soundEmitter.IsLoop()) return;

            soundEmitter.OnSoundFinishedPlaying -= OnSoundFinishedPlaying;
            soundEmitter.Stop();
            _pool.Release(soundEmitter);
        }

        private bool IsAudioPlaying() => _musicSoundEmitter != null && _musicSoundEmitter.IsPlaying();
    }
}