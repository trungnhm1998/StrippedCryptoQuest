using CryptoQuest.Audio.AudioData;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor
{
    [CustomEditor(typeof(AudioCueEventChannelSO))]
    public class AudioCueEventChannelSOEditor : Editor
    {
        public VisualTreeAsset Uxml;

        private AudioCueSO _audioCue;
        private Button _playButton;
        private AudioCueEventChannelSO Target => target as AudioCueEventChannelSO;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            Uxml.CloneTree(root);

            _playButton = root.Q<Button>("play-button");
            _playButton.SetEnabled(Application.isPlaying);
            _playButton.clicked += PlayClicked;

            var objectField = root.Q<ObjectField>("cue-object");
            objectField.objectType = typeof(AudioCueSO);
            objectField.RegisterValueChangedCallback(ChangeValueEditorCallback);

            return root;
        }

        private void ChangeValueEditorCallback(ChangeEvent<Object> evt) => _audioCue = (AudioCueSO)evt.newValue;
        private void PlayClicked() => Target.PlayAudio(_audioCue);

        private void OnDisable()
        {
            if (_playButton != null) _playButton.clicked -= PlayClicked;
        }
    }
}