using CryptoQuest.Audio.AudioData;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Tests.Editor.Audio
{
    public class AudioCueEventChannelSOTest
    {
        private static string AUDIO_CUE_PATH =
            "Assets/ScriptableObjects/Events/Audio/PlayMusicBackGroundChannel.asset";

        private static string AUDIO_CONFIGURATION_PATH =
            "Assets/ScriptableObjects/Events/Settings/Audio.asset";

        private AudioCueEventChannelSO _audioCueEventChannel;
        private AudioCueSO _audioCueSO;


        [SetUp]
        public void Setup()
        {
            _audioCueEventChannel = AssetDatabase.LoadAssetAtPath<AudioCueEventChannelSO>(AUDIO_CUE_PATH);
            _audioCueSO = ScriptableObject.CreateInstance<AudioCueSO>();
        }

        [Test]
        public void PlayAudio_CueIsValid_ShouldReturnsTrue()
        {
            bool raiseEvent = false;

            UnityAction<AudioCueSO> action = (cue) => { raiseEvent = true; };
            _audioCueEventChannel.AudioPlayRequested += action;

            _audioCueEventChannel.PlayAudio(_audioCueSO);

            Assert.IsTrue(raiseEvent);

            _audioCueEventChannel.AudioPlayRequested -= action;
        }

        [Test]
        public void StopAudio_CueIsValid_ShouldReturnsTrue()
        {
            bool raiseEvent = false;

            UnityAction<AudioCueSO> action = (cue) => { raiseEvent = true; };
            _audioCueEventChannel.AudioStopRequested += action;

            _audioCueEventChannel.StopAudio(_audioCueSO);

            Assert.IsTrue(raiseEvent);

            _audioCueEventChannel.AudioStopRequested -= action;
        }
    }
}