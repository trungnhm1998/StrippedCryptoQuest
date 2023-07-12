using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace CryptoQuest
{
    public class DialogTransmiter : MonoBehaviour
    {
        private DialogueRunner _dialogueRunner;
        [SerializeField] private StringEventChannelSO _onTriggerEndDialogue;

        private void Awake()
        {
            _dialogueRunner = FindObjectOfType<DialogueRunner>();
            _dialogueRunner.AddCommandHandler<string>("EndDialogue", OnDialogueEnd);
        }

        private void OnDialogueEnd(string str)
        {
            _onTriggerEndDialogue.RaiseEvent(str);
            Debug.Log("Completed event " + str);
        }
    }
}