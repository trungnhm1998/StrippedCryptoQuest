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
        public int CurrentHP;
        public int MaxHP;
        public int CurrentMP;
        public int MaxMP;
        public int CurrentEXP;
        public int MaxEXP;
        public string Class;
        public ElementalMockDataSO Elemental;
        public Sprite Avatar;
    }
}
