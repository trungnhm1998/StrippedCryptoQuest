using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public class SingleEntityInteractionManager : MonoBehaviour, IInteractionManager
    {
        private readonly List<GameObject> _potentialInteractions = new();

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

        private void AddPotentialInteraction(GameObject go) => _potentialInteractions.Add(go);
        private void RemovePotentialInteraction(GameObject go) => _potentialInteractions.Remove(go);

        public void Interact()
        {
            foreach (var go in _potentialInteractions)
            {
                go.TryGetComponent<IInteractable>(out var currentInteraction);
                if (currentInteraction == null) continue;
                currentInteraction.Interact();
            }
        }
    }
}