using CryptoQuest.Audio.Settings;
using DG.Tweening;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace CryptoQuest.Audio.AudioEmitters
{
    [RequireComponent((typeof(AudioSource)))]
    public class AudioEmitter : MonoBehaviour
    {
        public event UnityAction<AudioEmitterValue> AudioFinishedPlaying;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSettingsSO _setting;

        /// <summary>
        /// This will be the runtime value of the AudioCueSO
        /// a cueSO could be the same but value should be different
        /// e.g. gun shot SFX
        /// </summary>
        [ReadOnly] private AudioEmitterValue _emitterValue;

        private IObjectPool<AudioEmitter> _objectPool;
        public void Init(IObjectPool<AudioEmitter> pool) => _objectPool = pool;

        public void PlayAudioClip(AudioClip clip, bool hasLoop)
        {
            _emitterValue = new AudioEmitterValue(this);

            _audioSource.clip = clip;
            _audioSource.volume = _setting.Volume;
            _audioSource.loop = hasLoop;
            _audioSource.time = 0f;
            _audioSource.Play();

            if (hasLoop) return;
            Invoke(nameof(OnFinishedPlay), clip.length);
        }

        public void FadeMusicIn(AudioClip clip, float duration, float startTime = 0f)
        {
            PlayAudioClip(clip, true);
            _audioSource.volume = 0f;

            if (startTime <= _audioSource.clip.length)
            {
                _audioSource.time = startTime;
            }

            _audioSource.DOFade(_setting.Volume, duration);
        }

        public float FadeMusicOut(float duration)
        {
            _audioSource.DOFade(0f, duration).onComplete += OnFinishedPlay;

            return _audioSource.time;
        }

        public void Finish()
        {
            if (!_audioSource.loop) return;

            _audioSource.loop = false;
            float timeRemaining = _audioSource.clip.length - _audioSource.time;
            Invoke(nameof(OnFinishedPlay), timeRemaining);
        }

        public void Resume() => _audioSource.Play();
        public void Pause() => _audioSource.Pause();
        public void Stop() => _audioSource.Stop();
        public AudioClip GetClip() => _audioSource.clip;
        public bool IsPlaying() => _audioSource.isPlaying;
        public bool IsLoop() => _audioSource.loop;
        public void SetVolume(float value) => _audioSource.volume = value;
        public void ReleasePool() => _objectPool?.Release(this);
        private void OnFinishedPlay() => AudioFinishedPlaying?.Invoke(_emitterValue);
    }
}