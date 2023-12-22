using UnityEngine;

namespace CryptoQuest.Character
{
    public interface IInteractableOnTouch
    {
        public void Interact(GameObject sourceGO);
    }
}