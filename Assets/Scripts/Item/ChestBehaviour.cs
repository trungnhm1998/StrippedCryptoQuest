using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using CryptoQuest.Character;
using UnityEngine;

namespace CryptoQuest
{
    public class ChestBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueScriptableObject _dialogue;
        [SerializeField] private DialogueEventChannelSO _dialogEventChannel;
        [SerializeField] private Animator _animator;
        private bool _isOpen = false;

        public void Interact()
        {
            if (!_isOpen)
            {
                _animator.SetBool("OpenChest", true);
                _dialogEventChannel.Show(_dialogue);
                _isOpen = true;
            }
        }
    }
}
