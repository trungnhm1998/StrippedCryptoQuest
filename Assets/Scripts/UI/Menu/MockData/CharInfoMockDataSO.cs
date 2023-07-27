using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MockData
{
    [CreateAssetMenu(menuName = "Gameplay/MockData/Character Info")]
    public class CharInfoMockDataSO : ScriptableObject
    {
        public string Name;
        public int Level;
        public float CurrentHP;
        public float MaxHP;
        public float CurrentMP;
        public float MaxMP;
        public float CurrentEXP;
        public float MaxEXP;
        public string Class;
        public ElementalMockDataSO Elemental;
        public Sprite Avatar;
    }
}
