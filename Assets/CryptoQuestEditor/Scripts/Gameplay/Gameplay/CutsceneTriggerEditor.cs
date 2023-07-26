using CryptoQuest.Gameplay.Cutscenes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.Gameplay.Gameplay
{
    [CustomEditor(typeof(CutsceneTrigger))]
    public class CutsceneTriggerEditor : Editor
    {
        public VisualTreeAsset QuestDefinitionEditorUxml;
        private Button _playCutsceneButton;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            QuestDefinitionEditorUxml.CloneTree(root);

            _playCutsceneButton = root.Q<Button>("play-cutscene-button");
            _playCutsceneButton.SetEnabled(Application.isPlaying);
            _playCutsceneButton.clicked += PlayCutsceneClicked;

            return root;
        }

        private void PlayCutsceneClicked()
        {
            CutsceneTrigger cutsceneTrigger = (CutsceneTrigger)target;
            cutsceneTrigger.PlayCutscene();
        }
    }
}