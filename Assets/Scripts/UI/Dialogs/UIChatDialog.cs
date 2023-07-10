using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Dialogs
{
    // public class SpeechDialogArgs : DialogArgs
    // {
    //     public DialogueScriptableObject DialogueSO;
    // }

    public class UIChatDialog : ModalWindow<UIChatDialog>
    {
        [Header("Child Dialog")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private GameObject _content;
        [SerializeField] private LocalizeStringEvent _dialogLabel;

        public override UIChatDialog SetHeader(string text)
        {
            return null;
        }

        public override UIChatDialog SetBody(string text)
        {
            return null;
        }

        protected override void CheckIgnorableForClose()
        {
            return;
        }

        public override UIChatDialog Show()
        {
            // if (_dialogue.LinesCount == 0)
            // {
            //     Hide();
            //     return;
            // }

            _content.SetActive(true);

            return Instance;
        }

        // public new static UIChatDialog Create(bool ignorable = true)
        // {
        //     var name = typeof(UIChatDialog).Name.ToString();
        //     var modalWindow = Instantiate(gameObject);
        //     modalWindow.name = name;
        //     modalWindow.Instance = modalWindow;

        //     return modalWindow.Instance;
        // }
    }
}
