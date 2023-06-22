using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest
{
    [CreateAssetMenu(menuName = "Player Position Info")]
    public class PlayerPositionInfoSO : ScriptableObject
    {
        public Vector2 playerPosition;
        public Vector2 playerFacingAxis;
    }
}
