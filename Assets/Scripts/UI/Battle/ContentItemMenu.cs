using System;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace CryptoQuest.UI.Battle
{
    public abstract class ContentItemMenu : MonoBehaviour
    {
        public class BarDataStructure { }

        public event Action<BarDataStructure> barRaised;

        public abstract BarDataStructure Foo();
        public abstract void Init(BarDataStructure input);

        public void RaiseBar()
        {
            barRaised?.Invoke(Foo());
        }
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