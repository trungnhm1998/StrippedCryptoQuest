using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public class InteractOnTouch : MonoBehaviour
    {
        /// <summary>
        /// There should be a child InteractionZone game object with a trigger collider and ZoneTriggerController component
        /// </summary>
        /// <param name="entered"></param>
        /// <param name="go"></param>
        public void OnTriggerChangeDetected(bool entered, GameObject go)
        {
            if (!entered) return;

            var canInteract = go.TryGetComponent<IInteractableOnTouch>(out var interactObject);
            if (!canInteract) return;
            
            interactObject.Interact(gameObject);
        }
    }
}