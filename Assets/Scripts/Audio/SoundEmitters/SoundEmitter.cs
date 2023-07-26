using System.Collections;
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

        private IObjectPool<SoundEmitter> _objectPool = default;
        public IObjectPool<SoundEmitter> ObjectPool { get; set; }

        public void PlayAudioClip(AudioClip clip, bool hasLoop)
        {
            _audioSource.clip = clip;
            _audioSource.loop = hasLoop;
            _audioSource.time = 0f;
            _audioSource.Play();

            if (hasLoop) return;
            StartCoroutine(FinishedPlaying(clip.length));
        }

        public void FadeMusicIn(AudioClip clip, float duration, float startTime = 0f)
        {
            PlayAudioClip(clip, true);
            _audioSource.volume = 0f;

            if (startTime <= _audioSource.clip.length)
            {
                _audioSource.time = startTime;
            }

            //TODO: SETTING VOLUME 
            float END_VALUE_FOR_SOUND = 1;
            _audioSource.DOFade(END_VALUE_FOR_SOUND, duration);
        }

        public float FadeMusicOut(float duration)
        {
            _audioSource.DOFade(0f, duration).onComplete += NotifySoundDone;

            return _audioSource.time;
        }


        public void Finish()
        {
            if (!_audioSource.loop) return;

            _audioSource.loop = false;
            float timeRemaining = _audioSource.clip.length - _audioSource.time;
            StartCoroutine(FinishedPlaying(timeRemaining));
        }

        public void Resume()
        {
            _audioSource.Play();
        }

        public void Pause()
        {
            _audioSource.Pause();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        public AudioClip GetClip()
        {
            return _audioSource.clip;
        }

        public bool IsPlaying()
        {
            return _audioSource.isPlaying;
        }

        public bool IsLoop()
        {
            return _audioSource.loop;
        }

        public void ReleasePool()
        {
            _objectPool?.Release(this);
        }

        private IEnumerator FinishedPlaying(float clipLength)
        {
            yield return new WaitForSeconds(clipLength);

            NotifySoundDone();
        }

        private void NotifySoundDone()
        {
            OnSoundFinishedPlaying?.Invoke(this);
        }
    }
}