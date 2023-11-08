using System;
using System.Collections.Generic;
using CryptoQuest.Character;
using UnityEngine;

namespace CryptoQuest.ChangeClass.ScriptableObjects
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

    [CreateAssetMenu(menuName = "Crypto Quest/Character/Change Class")]
    public class ChangeClassSO : ScriptableObject
    {
        [field: SerializeField] public CharacterClass CharacterClass { get; private set; }
        [field: SerializeField] public List<ClassMaterial> ClassMaterials { get; private set; }
        [field: SerializeField] public int ItemMaterialId { get; private set; }
        [field: SerializeField] public int MaterialQuantity { get; private set; } = 1;
    }
}
