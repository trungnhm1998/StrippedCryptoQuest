using CryptoQuest.System.Dialogue.Events;
using CryptoQuest.System.Dialogue.Managers;
using CryptoQuest.System.Dialogue.YarnManager;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Character.DialogueProviders
{
    public class YarnDialogueProvider : DialogueProviderBehaviour
    {
        [field: Header("Yarn Config"), SerializeField]
        private VoidEventChannelSO _sceneLoaded;
        [field: SerializeField] public YarnProjectConfigSO YarnProjectConfig { get; private set; }

        [field: FormerlySerializedAs("_yarnNodeName"), SerializeField, HideInInspector]
        public string YarnNodeName { get; private set; } = "Start";

        private void OnEnable()
        {
            _sceneLoaded.EventRaised += OnConfigureYarn;
        }

        private void OnDisable()
        {
            _sceneLoaded.EventRaised -= OnConfigureYarn;
        }

        private void OnConfigureYarn()
        {
            if (YarnProjectConfig == null) return;
            YarnSpinnerDialogueManager.YarnProjectRequested?.Invoke(YarnProjectConfig);
        }

        public override void ShowDialogue()
        {
            YarnSpinnerDialogueManager.PlayDialogueRequested?.Invoke(YarnNodeName);
        }

#if UNITY_EDITOR
        public void Editor_SetYarnNodeName(string yarnNodeName) => YarnNodeName = yarnNodeName;
#endif
    }
}