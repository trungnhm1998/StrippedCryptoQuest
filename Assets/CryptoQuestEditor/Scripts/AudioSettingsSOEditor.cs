using CryptoQuest.Audio.Settings;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor
{
    [CustomEditor(typeof(AudioSettingSO))]
    public class AudioSettingsSOEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;

        private AudioSettingSO Target => target as AudioSettingSO;

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