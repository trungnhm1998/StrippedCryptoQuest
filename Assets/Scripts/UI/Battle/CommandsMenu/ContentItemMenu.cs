using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public abstract class ContentItemMenu : MonoBehaviour
    {
        public class BarDataStructure { }

        public abstract void Init(BarDataStructure input);
    }

    public class Mob
    {
        public string name;
    }

    public class SkillName
    {
        public string name;
    }

    public class ItemName
    {
        public string name;
    }
}