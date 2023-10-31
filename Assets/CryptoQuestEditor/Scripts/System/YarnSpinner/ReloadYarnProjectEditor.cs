using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack;
using CryptoQuest.System.Dialogue.YarnManager;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
using Yarn.Unity.Editor;

namespace CryptoQuestEditor.System.YarnSpinner
{
    public class ReloadYarnProjectEditor : EditorWindow
    {
        private const string ASSETS_SCENES_MAPS = "Assets/Scenes/Maps";
        private const string ASSETS_DIALOGUE_PATH = "Assets/Dialogues";
        private const string DEFAULT_NAME = "yarnConfigClone";
        private const string YARN_IMPORTER_NAME = "Yarn Importer";

        [SerializeField] private VisualTreeAsset _visualTreeAsset;

        /// <summary>
        /// Single Option
        /// </summary>
        private ObjectField _yarnProjectConfig;

        private ObjectField _cutscenePrefab;
        private Button _replaceButton;
        private Button _clearButton;

        /// <summary>
        /// Multiple Option
        /// </summary>
        private ObjectField _yarnConfigs;

        private IntegerField _indexYarnConfig;
        private ScrollView _scrollView;

        private TextField _pathName;
        private Button _replaceAllButton;
        private Button _initDialogueButton;

        private TabbedMenuController _tabbedMenuController;

        private StringBuilder _dependenciesString;
        private string _targetPath;
        private GameObject _currentGo;
        private readonly List<ObjectField> _objectFields = new();

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
            _tabbedMenuController = new TabbedMenuController(rootVisualElement);
            _tabbedMenuController.RegisterTabCallbacks();

            // Single Option
            _yarnProjectConfig = rootVisualElement.Q<ObjectField>("yarnProjectConfig");
            _cutscenePrefab = rootVisualElement.Q<ObjectField>("cutsceneAssets");
            _replaceButton = rootVisualElement.Q<Button>("replaceButton");
            _clearButton = rootVisualElement.Q<Button>("clearButton");

            // Multiple Option
            _yarnConfigs = rootVisualElement.Q<ObjectField>("yarnConfig");
            _indexYarnConfig = rootVisualElement.Q<IntegerField>("indexConfigYarn");
            _scrollView = rootVisualElement.Q<ScrollView>("yarnConfigScrollView");
            _pathName = rootVisualElement.Q<TextField>("pathName");
            _initDialogueButton = rootVisualElement.Q<Button>("initDialogueButton");
            _replaceAllButton = rootVisualElement.Q<Button>("replaceAllButton");

            _indexYarnConfig.RegisterValueChangedCallback(InitializeConfigYarnIndex);
            _objectFields.Add(_yarnConfigs);

            // Single Option
            _replaceButton.clicked += Replace;
            _clearButton.clicked += Clear;

            // Multiple Option
            _initDialogueButton.clicked += InitialDialogueData;
            _replaceAllButton.clicked += ReplaceAll;
        }

        private void InitializeConfigYarnIndex(ChangeEvent<int> evt)
        {
            _scrollView.Clear();
            _objectFields.Clear();

            _objectFields.Add(_yarnConfigs);

            int index = _indexYarnConfig.value - 1;

            if (index < 0) return;

            for (int i = 0; i < index; i++)
            {
                ObjectField newObjectField = new ObjectField(YARN_IMPORTER_NAME);
                string name = $"{DEFAULT_NAME}_{i}";

                newObjectField.objectType = typeof(TextAsset);
                newObjectField.name = name;

                _objectFields.Add(newObjectField);
            }

            _objectFields.ForEach(field => _scrollView.Add(field));
        }

        private void InitialDialogueData()
        {
            string pathName = _pathName.value;
            string folderPath = $"{ASSETS_DIALOGUE_PATH}/{pathName}";


            foreach (var objectField in _objectFields)
            {
                TextAsset obj = objectField.value as TextAsset;


                if (!AssetDatabase.IsValidFolder(folderPath))
                {
                    AssetDatabase.CreateFolder(ASSETS_DIALOGUE_PATH, pathName);
                }

                string assetPath = AssetDatabase.GetAssetPath(obj);
                AssetDatabase.CopyAsset(assetPath, folderPath);
                AssetDatabase.MoveAsset(assetPath, $"{folderPath}/{obj.name}.yarn");
            }

            YarnProjectConfigSO yarnProjectConfig = CreateInstance<YarnProjectConfigSO>();
            yarnProjectConfig.name = _pathName.value;

            AssetDatabase.CreateAsset(yarnProjectConfig, $"{folderPath}/{yarnProjectConfig.name}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            YarnEditorUtility.CreateYarnProject();
        }

        private void Clear()
        {
            _cutscenePrefab.value = null;
            _yarnProjectConfig.value = null;
        }

        private void ReplaceAll()
        {
            Debug.Log("ReplaceAll");
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