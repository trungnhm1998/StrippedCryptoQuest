using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.BaseGameplayData;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Character
{
    [Serializable]
    public class ClassMaterial
    {
        public int Id;
        public int Level;
        public ClassMaterial(int id, int level)
        {
            Id = id;
            Level = level;
        }
    }

    public class CharacterClass : GenericData
    {
        [field: SerializeField] public AssetLabelReference Label { get; private set; }
        [field: SerializeField] public List<ClassMaterial> ClassMaterials { get; private set; }
        [field: SerializeField] public int ItemMaterialId { get; private set; }
        [field: SerializeField] public int MaterialQuantity { get; private set; } = 1;
    }
}