using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MockData
{
    [CreateAssetMenu(menuName = "Gameplay/MockData/Elemental")]
    public class ElementalMockDataSO : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
    }
}
