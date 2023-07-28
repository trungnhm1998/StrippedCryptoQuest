using CryptoQuest.Audio;
using CryptoQuest.Audio.AudioData;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Audio
{
    public class AudioCueEventChannelSOTest
    {
        private AudioCueEventChannelSO _audioCueEventChannel;
        private AudioConfigurationSO _audioConfiguration;
        private AudioCueSO _audioCueSO;

        private static string AUDIO_CUE_PATH =
            "Assets/ScriptableObjects/Audio/Config/PlayMusicBackGround_Channel.asset";

        private static string AUDIO_CONFIGURATION_PATH =
            "Assets/ScriptableObjects/Audio/Config/AudioConfigurationSO.asset";

        [SetUp]
        public void Setup()
        {
            _audioCueEventChannel = AssetDatabase.LoadAssetAtPath<AudioCueEventChannelSO>(AUDIO_CUE_PATH);
            _audioConfiguration = AssetDatabase.LoadAssetAtPath<AudioConfigurationSO>(AUDIO_CONFIGURATION_PATH);
            _audioCueSO = ScriptableObject.CreateInstance<AudioCueSO>();
        }

        [Test]
        public void RaisePlayEvent_NoListeners_ShouldReturnsInvalidAudioCueKey()
        {
            var result = _audioCueEventChannel.RaisePlayEvent(_audioCueSO, _audioConfiguration);

            Assert.AreEqual(AudioCueKey.Invalid, result);
        }

        [Test]
        public void RaiseStopEvent_NoListeners_ShouldReturnsFalse()
        {
            var result = _audioCueEventChannel.RaiseStopEvent(AudioCueKey.Invalid);

            Assert.IsFalse(result);
        }

        [Test]
        public void RaiseFinishEvent_NoListeners_ShouldReturnsFalse()
        {
            var result = _audioCueEventChannel.RaiseFinishEvent(AudioCueKey.Invalid);

            Assert.IsFalse(result);
        }

        [Test]
        public void RaisePlayEvent_ValidInput_CallsOnAudioCuePlayRequested()
        {
            AudioCueKey expectedAudioCueKey = new AudioCueKey(123, _audioCueSO);

            bool eventRaised = false;
            _audioCueEventChannel.OnAudioCuePlayRequested += (audioCue, audioConfig) =>
            {
                eventRaised = true;
                return expectedAudioCueKey;
            };

            var result = _audioCueEventChannel.RaisePlayEvent(_audioCueSO, _audioConfiguration);

            Assert.IsTrue(eventRaised);
            Assert.AreEqual(expectedAudioCueKey, result);
        }

        [Test]
        public void RaiseStopEvent_ValidInput_CallsOnAudioCueStopRequested()
        {
            AudioCueKey key = new AudioCueKey(777, _audioCueSO);

            bool eventRaised = false;
            _audioCueEventChannel.OnAudioCueStopRequested += (audioCueKey) =>
            {
                eventRaised = true;
                return key == audioCueKey;
            };

            var result = _audioCueEventChannel.RaiseStopEvent(key);

            Assert.IsTrue(eventRaised);
            Assert.IsTrue(result);
        }

        [Test]
        public void RaiseFinishEvent_ValidInput_CallsOnAudioCueFinishRequested()
        {
            AudioCueKey key = new AudioCueKey(666, _audioCueSO);

            bool eventRaised = false;
            _audioCueEventChannel.OnAudioCueFinishRequested += (audioCueKey) =>
            {
                eventRaised = true;
                return key == audioCueKey;
            };

            var result = _audioCueEventChannel.RaiseFinishEvent(key);

            Assert.IsTrue(eventRaised);
            Assert.IsTrue(result);
        }
    }
}