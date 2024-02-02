using CryptoQuest.Audio.Settings;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor
{
    [CustomEditor(typeof(AudioSettingSO))]
    public class AudioSettingsSOEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;

        private AudioSettingSO Target => target as AudioSettingSO;
        private Slider volumeSlider;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            _visualTreeAsset.CloneTree(root);

            volumeSlider = root.Q<Slider>("volume-slider");
            volumeSlider.value = Target.Volume;

            volumeSlider.RegisterValueChangedCallback(ChangeValueEditorCallback);
            Target.VolumeChanged += ChangeMasterVolume;

            return root;
        }

        private void ChangeValueEditorCallback(ChangeEvent<float> evt) => Target.Volume = evt.newValue;
        private void ChangeMasterVolume(float value) => volumeSlider.value = value;
        private void OnDisable() => Target.VolumeChanged -= ChangeMasterVolume;
    }
}