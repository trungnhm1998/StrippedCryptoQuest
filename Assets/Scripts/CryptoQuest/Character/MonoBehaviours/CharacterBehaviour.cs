using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public abstract class CharacterBehaviour : MonoBehaviour
    {
        public enum EFacingDirection
        {
            South = 0,
            West = 1,
            North = 2,
            East = 3,
        }

        protected EFacingDirection FacingDirection = EFacingDirection.South;
    }
}

