using CryptoQuest.Audio.Settings;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace CryptoQuest.Audio.AudioEmitters
{
    [RequireComponent((typeof(AudioSource)))]
    public class AudioEmitter : MonoBehaviour
    {
        public event UnityAction<AudioEmitter> AudioFinishedPlaying;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSettingsSO _setting;

        private IObjectPool<AudioEmitter> _objectPool;
        public void Init(IObjectPool<AudioEmitter> pool) => _objectPool = pool;

        public void PlayAudioClip(AudioClip clip, bool hasLoop)
        {
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

        public void ReleasePool()
        {
            _objectPool?.Release(this);
        }


        private void OnFinishedPlay()
        {
            AudioFinishedPlaying?.Invoke(this);
        }
    }
}