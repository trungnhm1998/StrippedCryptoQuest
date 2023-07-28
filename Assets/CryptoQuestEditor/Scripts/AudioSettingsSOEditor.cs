using CryptoQuest.Audio.Settings;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor
{
    [CustomEditor(typeof(AudioSettingsSO))]
    public class AudioSettingsSOEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;

        private AudioSettingsSO Target => target as AudioSettingsSO;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            // draw default inspector
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _visualTreeAsset.CloneTree(root);

            var volumeSlider = root.Q<Slider>("volume-slider");
            volumeSlider.value = Target.Volume;
            volumeSlider.RegisterValueChangedCallback(evt => Target.Volume = evt.newValue);

            return root;
        }
    }
}