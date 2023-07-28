using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace CryptoQuest.Audio.SoundEmitters
{
    [RequireComponent((typeof(AudioSource)))]
    public class SoundEmitter : MonoBehaviour
    {
        public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;

        [SerializeField] private AudioSource _audioSource;

        private IObjectPool<SoundEmitter> _objectPool;
        public void Init(IObjectPool<SoundEmitter> pool) => _objectPool = pool;

        public void PlayAudioClip(AudioClip clip, AudioConfigurationSO setting, bool hasLoop)
        {
            _audioSource.clip = clip;
            setting.ApplyTo(_audioSource);
            _audioSource.loop = hasLoop;
            _audioSource.time = 0f;
            _audioSource.Play();

            if (hasLoop) return;
            Invoke(nameof(OnFinishedPlay), clip.length);
        }

        public void FadeMusicIn(AudioClip clip, AudioConfigurationSO setting, float duration, float startTime = 0f)
        {
            PlayAudioClip(clip, setting, true);
            _audioSource.volume = 0f;

            if (startTime <= _audioSource.clip.length)
            {
                _audioSource.time = startTime;
            }

            _audioSource.DOFade(setting.Volume, duration);
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

        public void ReleasePool()
        {
            _objectPool?.Release(this);
        }


        private void OnFinishedPlay()
        {
            OnSoundFinishedPlaying?.Invoke(this);
        }
    }
}