using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    /// <summary>
    /// https://docs.google.com/spreadsheets/d/1WkX1DyDOGf6EiAppo8Buz2sUkSKV5OnDENEvmHzKXNQ/edit#gid=1398474039
    /// </summary>
    [CreateAssetMenu(fileName = "LootTable", menuName = "Gameplay/Loot/Loot Table")]
    public class LootTable : ScriptableObject
    {
        [field: SerializeField] public string ID { get; private set; }
        [SerializeReference] public List<LootInfo> LootInfos = new();
    }
}