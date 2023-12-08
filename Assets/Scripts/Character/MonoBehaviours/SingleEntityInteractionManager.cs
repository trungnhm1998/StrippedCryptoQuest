using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public class SingleEntityInteractionManager : MonoBehaviour, IInteractionManager
    {
        private readonly List<IInteractable> _interactions = new();

        private void OnDisable() => _interactions.Clear();

        /// <summary>
        /// There should be a child InteractionZone game object with a trigger collider and ZoneTriggerController component
        /// </summary>
        /// <param name="entered"></param>
        /// <param name="go"></param>
        public void OnTriggerChangeDetected(bool entered, GameObject go)
        {
            if (entered)
                AddPotentialInteraction(go);
            else
                RemovePotentialInteraction(go);
        }

        private void AddPotentialInteraction(GameObject go)
        {
            var canInteract = go.TryGetComponent<IInteractable>(out var currentInteraction);

            if (!canInteract) return;
            _interactions.Add(currentInteraction);
        }

        private void RemovePotentialInteraction(GameObject go)
        {
            var canInteract = go.TryGetComponent<IInteractable>(out var currentInteraction);

            if (!canInteract) return;
            _interactions.Remove(currentInteraction);
        }

        public void Interact()
        {
            foreach (var go in _interactions) go?.Interact();
        }
    }
}