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
        public YarnProjectConfigSO YarnProjectConfig { get; private set; }

        [field: FormerlySerializedAs("_yarnNodeName"), SerializeField, HideInInspector]
        public string YarnNodeName { get; private set; } = "Start";

        public override void ShowDialogue()
        {
            if (YarnProjectConfig)
            {
                YarnSpinnerDialogueManager.YarnProjectRequested?.Invoke(YarnProjectConfig);
            }

            YarnSpinnerDialogueManager.PlayDialogueRequested?.Invoke(YarnNodeName);
        }

#if UNITY_EDITOR
        public void Editor_SetYarnNodeName(string yarnNodeName) => YarnNodeName = yarnNodeName;
#endif
    }
}