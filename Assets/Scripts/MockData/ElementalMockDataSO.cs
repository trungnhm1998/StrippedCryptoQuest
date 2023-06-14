using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest
{
    [CreateAssetMenu(menuName = "Gameplay/MockData/Elemental")]
    public class ElementalMockDataSO : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
    }
}
