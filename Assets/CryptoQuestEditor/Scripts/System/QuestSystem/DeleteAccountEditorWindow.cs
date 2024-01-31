using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Networking;
using IndiGames.Core.Common;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.CryptoQuestEditor.Scripts.System.QuestSystem
{
    [Serializable]
    public class ClearProfileResponse
    {
        public int code;
        public bool success;
        public string message;
        public int gold;
        public float diamond;
        public int soul;
        public long time;
        public Data data;

        [Serializable]
        public class Data
        {
            public string message;
        }
    }

    public class DeleteAccountEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;

        private bool _progressBarIsRunning;

        private TextField _uuidField, _emailField;
        private ProgressBar _progressBar;
        private Button _deleteButton;
        private HelpBox _helpBox;
        private VisualElement _progressBarContainer;

        [MenuItem("Crypto Quest/Delete Account")]
        public static DeleteAccountEditorWindow ShowWindow()
        {
            DeleteAccountEditorWindow[] windows = Resources.FindObjectsOfTypeAll<DeleteAccountEditorWindow>();
            if (windows.Length > 0) return windows[0];

            DeleteAccountEditorWindow window = GetWindow<DeleteAccountEditorWindow>();

            window.name = "Delete Account";
            window.titleContent = new GUIContent(window.name, EditorGUIUtility.IconContent("CacheServerDisconnected").image);
            window.ShowTab();
            
            return window;
        }

        private void OnEnable() => ApplyVisualElement();

        private void OnFocus() => ApplyVisualElement();

        private void OnLostFocus() => ApplyVisualElement();

        private void ApplyVisualElement()
        {
            rootVisualElement.Clear();
            rootVisualElement.Add(_visualTreeAsset.CloneTree());

            _uuidField = rootVisualElement.Q<TextField>("uuid-field");
            _emailField = rootVisualElement.Q<TextField>("email-field");

            _progressBar = rootVisualElement.Q<ProgressBar>("progress-bar");
            _progressBarContainer = _progressBar.Children().First().Children().First();

            _deleteButton = rootVisualElement.Q<Button>("delete-data-button");
            _helpBox = rootVisualElement.Q<HelpBox>("help-box");
            _helpBox.style.display = DisplayStyle.None;

            _deleteButton.clicked += DeleteAccount;

            if (Application.isPlaying) return;
            _helpBox.style.display = DisplayStyle.Flex;
            _uuidField.SetEnabled(false);
            _emailField.SetEnabled(false);
            _deleteButton.SetEnabled(false);
        }

        private void DeleteAccount()
        {
            string idValue = _uuidField.value;
            string emailValue = _emailField.value;

            var keyValue = string.IsNullOrEmpty(idValue) ? emailValue : idValue;
            var keyName = string.IsNullOrEmpty(idValue) ? "email" : "userId";

            if (string.IsNullOrEmpty(keyValue)) return;

            _progressBar.value = 0;
            _progressBarIsRunning = true;

            var restClient = ServiceProvider.GetService<IRestClient>();

            restClient
                .WithoutDispactError()
                .WithHeaders(new Dictionary<string, string> { { "DEBUG_KEY", Profile.DEBUG_KEY } })
                .WithParams(new Dictionary<string, string> { { keyName, keyValue } })
                .Request<ClearProfileResponse>(ERequestMethod.DELETE, "crypto/debug/game-data")
                .Subscribe(OnSucceed, OnError);

            RunProgressBar();
        }

        private void RunProgressBar()
        {
            _progressBar.title = "Deleting";
            _progressBarContainer.style.backgroundColor = Color.yellow;

            Observable.Interval(TimeSpan.FromSeconds(0.1f))
                .TakeWhile(_ => !_progressBarIsRunning)
                .Subscribe(_ => _progressBar.value++);
        }

        private void OnSucceed(ClearProfileResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;

            _progressBar.title = "Done";
            _progressBarContainer.style.backgroundColor = Color.green;
            _progressBar.value = 100;
            _progressBarIsRunning = false;

            Debug.Log($"<color=green>ClearProfileResponse.OnSucceed</color> {response.data.message}");
        }

        private void OnError(Exception exception)
        {
            _progressBar.title = "Error";
            _progressBarContainer.style.backgroundColor = Color.red;
            _progressBar.value = -1;
            _progressBarIsRunning = false;

            Debug.Log($"<color=red>ClearProfileResponse.OnError</color> {exception.Message}");
        }
    }
}