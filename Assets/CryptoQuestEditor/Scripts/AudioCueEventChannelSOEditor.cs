using CryptoQuest.Audio.AudioData;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using NotImplementedException = System.NotImplementedException;

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

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            Uxml.CloneTree(root);

            _playButton = root.Q<Button>("play-button");
            _playButton.SetEnabled(Application.isPlaying);

            var objectField = root.Q<ObjectField>("cue-object");
            objectField.objectType = typeof(AudioCueSO);
            objectField.RegisterValueChangedCallback(evt =>
            {
                _playButton.clicked += PlayClicked;
                _audioCue = (AudioCueSO)evt.newValue;
            });


            return root;
        }

        private void OnDisable()
        {
            _playButton.clicked -= PlayClicked;
        }

        private void PlayClicked()
        {
            Target.PlayAudio(_audioCue);
        }
    }
}