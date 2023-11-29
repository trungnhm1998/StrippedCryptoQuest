using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Events.UI.Menu
{
    [CreateAssetMenu(fileName = "HeroInventoryFilledEvent", menuName = "HeroInventoryFilledEvent", order = 0)]
    public class HeroInventoryFilledEvent : GenericEventChannelSO<List<HeroSpec>> { }
}