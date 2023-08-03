using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Audio;
using CryptoQuest.Audio.AudioData;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.Audio
{
    [TestFixture]
    [Category("Integration")]
    public class AudioManagerTest
    {
#if UNITY_EDITOR
        private List<AudioCueSO> _bgmCue;
        private List<AudioCueSO> _sfxCue;
        private const string AUDIO_TEST_SCENE = "Assets/Tests/Runtime/Audio/Audio.unity";
        private const string TEN_TIME_SFX_TEST_SCENE = "Assets/Tests/Runtime/Audio/TenTimeSFX.asset";

        private PlayMusicOnSceneLoaded _bgmHandle;
        private PlaySFX _sfxHandle;

        [UnitySetUp]
        public IEnumerator OneTimeSetup()
        {
            var audioBgmCueGUIDs = AssetDatabase.FindAssets("t:BGMCueSO");
            var audioSfxCueGUIDs = AssetDatabase.FindAssets("t:SFXCueSO");

            _bgmCue = new();
            _sfxCue = new();

            foreach (var guid in audioBgmCueGUIDs)
            {
                var audio = AssetDatabase.LoadAssetAtPath<BGMCueSO>(AssetDatabase.GUIDToAssetPath(guid));
                _bgmCue.Add(audio);
            }

            Assert.Greater(_bgmCue.Count, 0, "Bgm audio asset count must be greater than 0");

            foreach (var guid in audioSfxCueGUIDs)
            {
                var audio = AssetDatabase.LoadAssetAtPath<SFXCueSO>(AssetDatabase.GUIDToAssetPath(guid));
                _sfxCue.Add(audio);
            }

            Assert.Greater(_sfxCue.Count, 0, "Sfx audio asset count must be greater than 0");

            yield return EditorSceneManager.LoadSceneInPlayMode(AUDIO_TEST_SCENE,
                new LoadSceneParameters(LoadSceneMode.Single));

            _bgmHandle = Object.FindObjectOfType<PlayMusicOnSceneLoaded>();

            Assert.NotNull(_bgmHandle, "Cannot find PlayMusicOnSceneLoaded in scene");

            _sfxHandle = Object.FindObjectOfType<PlaySFX>();

            Assert.NotNull(_sfxHandle, "Cannot find PlaySFX in scene");
        }


        [UnityTest]
        public IEnumerator PlayBgm_ShouldPlayAudioAndStopAfter5Second()
        {
            _bgmHandle.MusicTrack = _bgmCue[0];
            Assert.AreEqual(_bgmCue[0], _bgmHandle.MusicTrack);

            _bgmHandle.PlayBackgroundMusic();

            var time = 0f;
            while (true)
            {
                time += Time.deltaTime;

                if (time >= 5.01f)
                {
                    _bgmHandle.StopBackgroundMusic();
                    break;
                }

                yield return null;
            }

            Assert.IsTrue(time >= 5f && time <= 5.1f, "Audio should stop after 5 seconds");
        }

        [UnityTest]
        public IEnumerator PlaySfx_ShouldPlay10Time()
        {
            SFXCueSO sfxTenTime = AssetDatabase.LoadAssetAtPath<SFXCueSO>(TEN_TIME_SFX_TEST_SCENE);
            _sfxHandle.SfxTrack = sfxTenTime;
            Assert.AreEqual(sfxTenTime, _sfxHandle.SfxTrack);

            int playTime = 0;

            for (int i = 0; i < 20; i++)
            {
                _sfxHandle.OnPlaySFX();
                yield return new WaitForSeconds(0.5f);
                playTime++;
            }

            Assert.AreEqual(20, playTime, "Audio should play 20 times");
        }

#endif
    }
}