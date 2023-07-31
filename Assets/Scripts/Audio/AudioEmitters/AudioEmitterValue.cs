using System;
using UnityEngine.Events;

namespace CryptoQuest.Audio.AudioEmitters
{
    [Serializable]
    public struct AudioEmitterValue
    {
        private AudioEmitter _audioEmitter;

        public AudioEmitterValue(AudioEmitter audioEmitter)
        {
            _audioEmitter = audioEmitter;
        }

        public void UnregisterEvent(UnityAction<AudioEmitterValue> audioFinishedPlaying)
        {
            if (_audioEmitter == null) return;
            _audioEmitter.AudioFinishedPlaying -= audioFinishedPlaying;
        }

        public void Stop()
        {
            if (_audioEmitter == null) return;
            _audioEmitter.Stop();
        }

        public void ReleaseToPool()
        {
            if (_audioEmitter == null) return;
            _audioEmitter.ReleasePool();
        }
    }
}