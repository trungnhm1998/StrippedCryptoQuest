using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack;
using CryptoQuest.System.Dialogue.YarnManager;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.System.YarnSpinner
{
    public class ReloadYarnProjectEditor : EditorWindow
    {
        private const string ASSETS_SCENES_MAPS = "Assets/Scenes/Maps";

        [SerializeField] private VisualTreeAsset _visualTreeAsset;

        private ObjectField _yarnProjectConfig;
        private ObjectField _cutscenePrefab;
        private Button _replaceButton;
        private Button _clearButton;

        private StringBuilder _dependenciesString;
        private string _targetPath;
        private GameObject _currentGo;

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

            _yarnProjectConfig = rootVisualElement.Q<ObjectField>("yarnProjectConfig");
            _cutscenePrefab = rootVisualElement.Q<ObjectField>("cutsceneAssets");
            _replaceButton = rootVisualElement.Q<Button>("replaceButton");
            _clearButton = rootVisualElement.Q<Button>("clearButton");

            _replaceButton.clicked += Replace;
            _clearButton.clicked += Clear;
        }

        private void Clear()
        {
            _cutscenePrefab.value = null;
            _yarnProjectConfig.value = null;
        }

        private void Replace()
        {
            GameObject currentGo = _cutscenePrefab.value as GameObject;
            YarnProjectConfigSO yarnProjectConfig = _yarnProjectConfig.value as YarnProjectConfigSO;

            if (currentGo == null)
            {
                Debug.LogError("Cutscene prefab is null");
                return;
            }

            if (yarnProjectConfig == null)
            {
                Debug.LogError("YarnProjectConfig is null");
                return;
            }

            _currentGo = currentGo;

            Debug.Log($"Start replacing {currentGo.name} with {yarnProjectConfig.name}");

            SearchObjectWithScenes(yarnProjectConfig);
            BindingData(yarnProjectConfig);

            PrefabUtility.SaveAsPrefabAsset(_currentGo, _targetPath);

            Debug.Log($"Finished replacing {currentGo.name} with {yarnProjectConfig.name}");
        }

        private void SearchObjectWithScenes(YarnProjectConfigSO yarnProjectConfig)
        {
            _targetPath = string.Empty;
            _dependenciesString = new();

            string targetPath = AssetDatabase.GetAssetPath(_currentGo);

            Debug.Log($"Binding {yarnProjectConfig.name} to {targetPath}");

            string[] allScenes = AssetDatabase.FindAssets("t:SceneAsset", new[] { ASSETS_SCENES_MAPS });

            foreach (string guid in allScenes)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(guid);
                string[] dependencies = AssetDatabase.GetDependencies(scenePath);

                if (!dependencies.Contains(targetPath)) continue;
                _dependenciesString.AppendLine(scenePath);
            }

            string path = _dependenciesString.ReplaceString("\r\n");

            if (SceneManager.GetActiveScene().path != path)
            {
                EditorSceneManager.OpenScene($"{path}", OpenSceneMode.Single);
            }

            _targetPath = targetPath;
        }

        private void BindingData(YarnProjectConfigSO yarnProjectConfig)
        {
            GameObject currentGo = GameObject.Find(_currentGo.name);

            PlayableDirector playableDirector = currentGo.GetComponentInChildren<PlayableDirector>();

            IEnumerable<PlayableBinding> assetOutputs = playableDirector.playableAsset.outputs;
            foreach (PlayableBinding binding in assetOutputs)
            {
                PlayableBinding playableBinding = binding;
                if (playableBinding.sourceObject is not YarnSpinnerNodeTrackAsset currentTrack) continue;

                playableDirector.SetGenericBinding(currentTrack, yarnProjectConfig);

                if (currentTrack == null) continue;

                IEnumerable<TimelineClip> clips = currentTrack.GetClips();
                foreach (TimelineClip clip in clips)
                {
                    YarnSpinnerNodePlayableAsset clipAsset = clip.asset as YarnSpinnerNodePlayableAsset;

                    if (clipAsset == null) continue;
                    if (clipAsset.OnYarnProjectConfigEvent != null) continue;

                    clipAsset.Editor_SetConfigEvent();
                }
            }

            _currentGo = currentGo;
        }
    }
}