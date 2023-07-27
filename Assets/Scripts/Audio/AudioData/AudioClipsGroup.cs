using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CryptoQuest.Audio.AudioData
{
    [Serializable]
    public class AudioClipsGroup
    {
        public ESequenceMode eSequenceMode = ESequenceMode.Sequential;
        public AudioClip[] audioClips;

        private int _nextClipToPlay = -1;
        private int _lastClipPlayed = -1;

        public AudioClip GetNextClip()
        {
            if (audioClips.Length == 1)
                return audioClips[0];

            switch (eSequenceMode)
            {
                case ESequenceMode.Random:
                    _nextClipToPlay = Random.Range(0, audioClips.Length);
                    break;

                case ESequenceMode.ImmediateRepeat:
                    do
                    {
                        _nextClipToPlay = Random.Range(0, audioClips.Length);
                    } while (_nextClipToPlay == _lastClipPlayed);

                    break;

                default:
                case ESequenceMode.Sequential:
                    _nextClipToPlay = (int)Mathf.Repeat(++_nextClipToPlay, audioClips.Length);
                    break;
            }

            _lastClipPlayed = _nextClipToPlay;
            return audioClips[_nextClipToPlay];
        }
    }
}