using CryptoQuest.Audio;
using CryptoQuest.Audio.AudioData;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Audio
{
    public class AudioCueEventChannelSOTest
    {
        // private AudioCueEventChannelSO _audioCueEventChannel;
        // private AudioSettingsSO _audioConfiguration;
        // private AudioCueSO _audioCueSO;
        //
        // private static string AUDIO_CUE_PATH =
        //     "Assets/ScriptableObjects/Audio/Config/PlayMusicBackGround_Channel.asset";
        //
        // private static string AUDIO_CONFIGURATION_PATH =
        //     "Assets/ScriptableObjects/Audio/Config/AudioConfigurationSO.asset";
        //
        // [SetUp]
        // public void Setup()
        // {
        //     _audioCueEventChannel = AssetDatabase.LoadAssetAtPath<AudioCueEventChannelSO>(AUDIO_CUE_PATH);
        //     _audioConfiguration = AssetDatabase.LoadAssetAtPath<AudioSettingsSO>(AUDIO_CONFIGURATION_PATH);
        //     _audioCueSO = ScriptableObject.CreateInstance<AudioCueSO>();
        // }
        //
        // [Test]
        // public void RaisePlayEvent_NoListeners_ShouldReturnsInvalidAudioCueKey()
        // {
        //     var result = _audioCueEventChannel.PlayAudio(_audioCueSO);
        //
        //     Assert.AreEqual(AudioCueKey.Invalid, result);
        // }
        //
        // [Test]
        // public void RaiseStopEvent_NoListeners_ShouldReturnsFalse()
        // {
        //     var result = _audioCueEventChannel.StopAudio(AudioCueKey.Invalid);
        //
        //     Assert.IsFalse(result);
        // }
        //
        // [Test]
        // public void RaiseFinishEvent_NoListeners_ShouldReturnsFalse()
        // {
        //     var result = _audioCueEventChannel.RaiseFinishEvent(AudioCueKey.Invalid);
        //
        //     Assert.IsFalse(result);
        // }
        //
        // [Test]
        // public void RaisePlayEvent_ValidInput_CallsOnAudioCuePlayRequested()
        // {
        //     AudioCueKey expectedAudioCueKey = new AudioCueKey(123, _audioCueSO);
        //
        //     bool eventRaised = false;
        //     _audioCueEventChannel.AudioPlayRequested += _ =>
        //     {
        //         eventRaised = true;
        //         return expectedAudioCueKey;
        //     };
        //
        //     var result = _audioCueEventChannel.PlayAudio(_audioCueSO);
        //
        //     Assert.IsTrue(eventRaised);
        //     Assert.AreEqual(expectedAudioCueKey, result);
        // }
        //
        // [Test]
        // public void RaiseStopEvent_ValidInput_CallsOnAudioCueStopRequested()
        // {
        //     AudioCueKey key = new AudioCueKey(777, _audioCueSO);
        //
        //     bool eventRaised = false;
        //     _audioCueEventChannel.AudioStopRequested += (audioCueKey) =>
        //     {
        //         eventRaised = true;
        //         return key == audioCueKey;
        //     };
        //
        //     var result = _audioCueEventChannel.StopAudio(key);
        //
        //     Assert.IsTrue(eventRaised);
        //     Assert.IsTrue(result);
        // }
        //
        // [Test]
        // public void RaiseFinishEvent_ValidInput_CallsOnAudioCueFinishRequested()
        // {
        //     AudioCueKey key = new AudioCueKey(666, _audioCueSO);
        //
        //     bool eventRaised = false;
        //     _audioCueEventChannel.AudioFinished += (audioCueKey) =>
        //     {
        //         eventRaised = true;
        //         return key == audioCueKey;
        //     };
        //
        //     var result = _audioCueEventChannel.RaiseFinishEvent(key);
        //
        //     Assert.IsTrue(eventRaised);
        //     Assert.IsTrue(result);
        // }
    }
}