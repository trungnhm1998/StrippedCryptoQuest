using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    [CreateAssetMenu(fileName = "LootTable", menuName = "Gameplay/Loot/Loot Table")]
    public class LootTable : ScriptableObject
    {
        [field: SerializeField] public string ID { get; private set; }
        [SerializeReference] public List<LootInfo> LootInfos = new();
    }
}