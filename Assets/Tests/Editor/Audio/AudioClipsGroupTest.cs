using CryptoQuest.Audio.AudioData;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Audio
{
    public class AudioClipsGroupTests
    {
        private const int NUMBER_OF_TRY = 10;
        private const int FEW_OF_CLIPS = 2;
        private const int MANY_OF_CLIPS = 5;
        private const int NUMBER_OF_PASS = 2;

        private AudioClip[] _clips;
        private AudioClipsGroup _group;

        [TestCase(ESequenceMode.Sequential)]
        [TestCase(ESequenceMode.Repeat)]
        [TestCase(ESequenceMode.Random)]
        public void AudioClipsGroup_GetNextClip_ExpectNotIndexOutOfRange(ESequenceMode mode)
        {
            _clips = CreateEmptyAudioClips(FEW_OF_CLIPS);
            _group = new(_clips, mode);
            ChangeClipsContinuously();
        }

        private void ChangeClipsContinuously()
        {
            for (int i = 0; i < NUMBER_OF_TRY; i++)
            {
                _group.SwitchToNextClip();
            }
        }

        [Test]
        public void AudioClipsGroup_GetNextClipSequently_ExpectCorrectSequence()
        {
            _clips = CreateEmptyAudioClips(MANY_OF_CLIPS);
            _group = new(_clips, ESequenceMode.Sequential);

            for (int i = 0; i < NUMBER_OF_PASS; i++)
            {
                TravelOnePassThroughClips();
            }
        }

        private void TravelOnePassThroughClips()
        {
            for (int i = 0; i < _clips.Length; i++)
            {
                Assert.That(_group.CurrentClip == _clips[i]);
                _group.SwitchToNextClip();
            }
        }

        private AudioClip[] CreateEmptyAudioClips(int quantity)
        {
            var newClips = new AudioClip[quantity];
            for (int i = 0; i < quantity; i++)
            {
                newClips[i] = AudioClip.Create("AudioClip" + i, 1, 1, 1000, false);
            }

            return newClips;
        }

        [TearDown]
        public void DestroyAudioClips()
        {
            for (int i = 0; i < _clips.Length; i++)
            {
                Object.DestroyImmediate(_clips[i]);
            }
        }
    }
}