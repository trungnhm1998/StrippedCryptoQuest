using System;
using UnityEngine;

namespace CryptoQuest.Audio.AudioData
{
    [Serializable]
    public class AudioClipsGroup
    {
        private readonly AudioClip[] _audioClips;
        private readonly IListIndex _clipIndex;

        public AudioClipsGroup(AudioClip[] audioClips, ESequenceMode mode = ESequenceMode.Sequential)
        {
            _audioClips = audioClips;
            _clipIndex = ListIndexFactory.Create(mode);
        }

        public AudioClip CurrentClip => _audioClips[_clipIndex.Value];

        public AudioClip SwitchToNextClip()
        {
            _clipIndex.GoForward(_audioClips.Length);
            return _audioClips[_clipIndex.Value];
        }
    }
}