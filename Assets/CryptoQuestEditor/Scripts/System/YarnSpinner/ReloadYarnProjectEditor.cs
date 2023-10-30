using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack;
using CryptoQuest.System.Dialogue.YarnManager;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.System.YarnSpinner
{
    public class ReloadYarnProjectEditor : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;

        private ObjectField _playableAssets;
        private ObjectField _yarnProjectConfig;
        private Button _replaceButton;
        private Button _clearButton;

        [MenuItem("Crypto Quest/Reload Yarn Project")]
        public static ReloadYarnProjectEditor ShowWindow()
        {
            var windows = Resources.FindObjectsOfTypeAll<ReloadYarnProjectEditor>();
            if (windows.Length > 0) return windows[0];

            var window = GetWindow<ReloadYarnProjectEditor>();
            return window;
        }

        private void OnEnable()
        {
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            rootVisualElement.Add(_visualTreeAsset.CloneTree());

            _playableAssets = rootVisualElement.Q<ObjectField>("playableAssets");
            _yarnProjectConfig = rootVisualElement.Q<ObjectField>("yarnProjectConfig");
            _replaceButton = rootVisualElement.Q<Button>("replaceButton");
            _clearButton = rootVisualElement.Q<Button>("clearButton");

            _replaceButton.clicked += Replace;
            _clearButton.clicked += Clear;
        }

        private void Clear()
        {
            _playableAssets.value = null;
            _yarnProjectConfig.value = null;
        }

        private void Replace()
        {
            var playableAssets = _playableAssets.value as PlayableAsset;
            var yarnProjectConfig = _yarnProjectConfig.value as YarnProjectConfigSO;

            if (playableAssets == null)
            {
                Debug.LogError("PlayableAsset is null");
                return;
            }

            if (yarnProjectConfig == null)
            {
                Debug.LogError("YarnProjectConfig is null");
                return;
            }

            IEnumerable<PlayableBinding> tracks = playableAssets.outputs;
            foreach (PlayableBinding track in tracks)
            {
                var playableBinding = track;
                if (playableBinding.sourceObject is YarnSpinnerNodeTrackAsset)
                {
                    Debug.Log($"Replacing {playableBinding.sourceObject}");
                    playableBinding.sourceObject = yarnProjectConfig;
                    Debug.Log($"Replaced {playableBinding.sourceObject}");
                }
                else
                {
                    Debug.LogWarning(
                        $"Skipped {playableBinding.sourceObject} as it is not of type YarnSpinnerNodeTrackAsset");
                }
            }
        }
    }
}