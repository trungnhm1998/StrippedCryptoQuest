using CryptoQuest.Battle.Components;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.ChangeClass.Interfaces
{
    public class IChangeClass : MonoBehaviour
    {
        public HeroBehaviour Hero { get; }
        public string DisplayName { get; }
        public Sprite Icon { get; }
    }
}