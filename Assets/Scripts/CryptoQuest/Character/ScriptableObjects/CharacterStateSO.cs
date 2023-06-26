using UnityEngine;

namespace CryptoQuest.Character.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Crypto Quest/Characters/Character State")]
    public class CharacterStateSO : ScriptableObject
    {
        public Character.EFacingDirection FacingDirection;
    }
}