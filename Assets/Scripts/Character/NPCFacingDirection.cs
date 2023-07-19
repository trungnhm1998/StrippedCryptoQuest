using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class NPCFacingDirection : CharacterBehaviour
    {
        private HeroBehaviour _heroFacingDirection;
        public void NPCInteract()
        {
            SetFacingDirection(_heroFacingDirection.gameObject.transform.position - transform.position);
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("InteractionZone") && _heroFacingDirection == null)
            {
                _heroFacingDirection = other.GetComponentInParent<HeroBehaviour>();
            }
        }
    }
}
