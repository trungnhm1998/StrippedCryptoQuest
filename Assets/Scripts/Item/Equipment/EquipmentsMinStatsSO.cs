using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    /// <summary>
    /// Store min stats of every equipments when fetch from server
    /// </summary>
    [CreateAssetMenu(fileName = "EquipmentsMinStats", menuName = "Crypto Quest/Inventory/Equipments Min Stats")]
    public class EquipmentsMinStatsSO : ScriptableObject
    {
        public Dictionary<int, AttributeWithValue[]> EquipmentsMinStats { get; set; } = new();

        private void OnEnable()
        {
            EquipmentsMinStats = new();
        }

        private void OnValidate()
        {
            EquipmentsMinStats = new();
        }
    }
}