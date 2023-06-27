using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;

namespace CryptoQuest.Character.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Crypto Quest/Characters/Character State")]
    public class CharacterStateSO : ScriptableObject
    {
        public CharacterBehaviour.EFacingDirection FacingDirection;
    }
}