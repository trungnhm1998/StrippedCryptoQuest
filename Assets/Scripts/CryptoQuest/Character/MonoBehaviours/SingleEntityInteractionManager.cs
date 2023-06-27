using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public class SingleEntityInteractionManager : MonoBehaviour, IInteractionManager
    {
        private IInteractable _currentInteraction;

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
            if (_currentInteraction != null)
            {
                Debug.LogWarning($"_currentInteraction is not null and will be overriden.");
            }
            _currentInteraction = go.GetComponent<IInteractable>();
        }

        private void RemovePotentialInteraction(GameObject go)
        {
            if (go.GetComponent<IInteractable>() != _currentInteraction) return;
            _currentInteraction = null;
        }

        public void Interact()
        {
            if (_currentInteraction == null) return;
            _currentInteraction.Interact();
        }
    }
}