using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public class InteractionManager : MonoBehaviour
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

        private void AddPotentialInteraction(GameObject go) { }

        private void RemovePotentialInteraction(GameObject go) { }
    }

    public interface IInteractable { }
}