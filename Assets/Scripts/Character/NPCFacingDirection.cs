using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class NPCFacingDirection : CharacterBehaviour
    {
        private Transform _other;
        public void FaceOther()
        {
            SetFacingDirection(_other.position - transform.position);
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("InteractionZone") && _other == null)
            {
                _other = other.transform;
            }
        }
    }
}
