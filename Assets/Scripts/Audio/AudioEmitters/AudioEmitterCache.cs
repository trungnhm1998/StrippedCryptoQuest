using System.Collections.Generic;
using CryptoQuest.Audio.AudioData;

namespace CryptoQuest.Audio.AudioEmitters
{
    public class AudioEmitterCache
    {
        private readonly Dictionary<AudioCueSO, List<AudioEmitter>> _cache = new();
        public Dictionary<AudioCueSO, List<AudioEmitter>> Cache => _cache;

        /// <summary>
        /// Adds a new association between an audio cue and an array of sound emitters.
        /// </summary>
        /// <param name="key">which cue getting played</param>
        /// <param name="emitters">emitters that created from clips in the cue</param>
        public void Add(AudioCueSO key, List<AudioEmitter> emitters)
        {
            _cache.Add(key, emitters);
        }

        public bool TryGetValue(AudioCueSO key, out List<AudioEmitter> emitter)
        {
            return _cache.TryGetValue(key, out emitter);
        }


        /// <summary>
        /// Removes the association between an audio cue key and its sound emitters from the store.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(AudioCueSO key)
        {
            return _cache.Remove(key);
        }
    }
}