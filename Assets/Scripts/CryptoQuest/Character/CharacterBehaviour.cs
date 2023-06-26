using CryptoQuest.Character.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class CharacterBehaviour : MonoBehaviour
    {
        [SerializeField] private CharacterStateSO _characterStateSO;
        public Character.EFacingDirection FacingDirection { set => _characterStateSO.FacingDirection = value; }
    }
}