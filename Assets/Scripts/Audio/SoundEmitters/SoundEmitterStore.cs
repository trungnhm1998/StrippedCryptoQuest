using System.Collections.Generic;
using CryptoQuest.Audio.AudioData;
using CryptoQuest.Audio.SoundEmitters;

namespace CryptoQuest.Audio
{
    public class SoundEmitterStore
    {
        private int _nextUniqueKey = 0;
        private List<AudioCueKey> _emittersKey;
        private List<SoundEmitter[]> _emittersList;

        public SoundEmitterStore()
        {
            _emittersList = new();
            _emittersKey = new();
        }

        /// Adds a new association between an audio cue key and an array of sound emitters.
        /// </summary>
        /// <param name="key">The AudioCueKey to associate with the sound emitters.</param>
        /// <param name="emitter">An array of SoundEmitter objects associated with the audio cue key.</param>
        public void Add(AudioCueKey key, SoundEmitter[] emitter)
        {
            _emittersKey.Add(key);
            _emittersList.Add(emitter);
        }

        /// <summary>
        /// Adds a new association between an audio cue and an array of sound emitters.
        /// </summary>
        /// <param name="cue">The AudioCueSO to associate with the sound emitters.</param>
        /// <param name="emitter">An array of SoundEmitter objects associated with the audio cue.</param>
        /// <returns>The AudioCueKey associated with the newly added audio cue and sound emitters.</returns>
        public AudioCueKey Add(AudioCueSO cue, SoundEmitter[] emitter)
        {
            AudioCueKey emitterKey = GetKey(cue);

            _emittersKey.Add(emitterKey);
            _emittersList.Add(emitter);

            return emitterKey;
        }

        /// <summary>
        /// Gets the sound emitters associated with the specified AudioCueKey.
        /// </summary>
        /// <param name="key">The AudioCueKey representing the audio cue to retrieve sound emitters for.</param>
        /// <param name="emitter">The array of SoundEmitter objects associated with the audio cue.</param>
        /// <returns>True if the sound emitters were found; otherwise, false.</returns>
        public bool Get(AudioCueKey key, out SoundEmitter[] emitter)
        {
            int index = _emittersKey.FindIndex(x => x == key);

            if (index < 0)
            {
                emitter = null;
                return false;
            }

            emitter = _emittersList[index];
            return true;
        }

        /// <summary>
        /// Removes the association between an audio cue key and its sound emitters from the store.
        /// </summary>
        /// <param name="key">The AudioCueKey representing the audio cue to remove.</param>
        /// <returns>True if the audio cue was found and removed; otherwise, false.</returns>
        public bool Remove(AudioCueKey key)
        {
            int index = _emittersKey.FindIndex(x => x == key);
            return RemoveAt(index);
        }

        private AudioCueKey GetKey(AudioCueSO cue)
        {
            return new AudioCueKey(_nextUniqueKey++, cue);
        }

        private bool RemoveAt(int index)
        {
            if (index < 0)
            {
                return false;
            }

            _emittersKey.RemoveAt(index);
            _emittersList.RemoveAt(index);

            return true;
        }
    }
}