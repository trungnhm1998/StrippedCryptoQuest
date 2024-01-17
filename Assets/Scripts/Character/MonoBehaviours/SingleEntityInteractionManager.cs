using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public class SingleEntityInteractionManager : MonoBehaviour, IInteractionManager
    {
        private Dictionary<IInteractable, float> _interactions = new Dictionary<IInteractable, float>();

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

            Vector2 thisColliderPosition = transform.position;
            Vector2 otherColliderPosition = go.transform.position;
            float distance = Vector2.Distance(thisColliderPosition, otherColliderPosition);

            if (!canInteract) return;
            _interactions.Add(currentInteraction, distance);
        }

        private void RemovePotentialInteraction(GameObject go)
        {
            var canInteract = go.TryGetComponent<IInteractable>(out var currentInteraction);

            if (!canInteract) return;
            _interactions.Remove(currentInteraction);
        }

        public void Interact()
        {
            KeyValuePair<IInteractable, float> smallestDistancePair = FindSmallestDistancePair();
            smallestDistancePair.Key?.Interact();
        }

        private KeyValuePair<IInteractable, float> FindSmallestDistancePair()
        {
            KeyValuePair<IInteractable, float> smallestDistancePair =
                new KeyValuePair<IInteractable, float>(null, float.MaxValue);

            foreach (var pair in _interactions)
            {
                if (pair.Value < smallestDistancePair.Value)
                {
                    smallestDistancePair = pair;
                }
            }
            return smallestDistancePair;
        }
    }
}