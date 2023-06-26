using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest
{
    [CreateAssetMenu(menuName = "Player State")]
    public class CharacterStateSO : ScriptableObject
    {
        public Character.EFacingDirection facingDirection;
    }
}