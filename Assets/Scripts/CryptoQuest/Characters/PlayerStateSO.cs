using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoQuest;

namespace CryptoQuest
{
    [CreateAssetMenu(menuName = "Player State")]
    public class PlayerStateSO : ScriptableObject
    {
        public Character.EFacingDirection facingDirection;
    }
}