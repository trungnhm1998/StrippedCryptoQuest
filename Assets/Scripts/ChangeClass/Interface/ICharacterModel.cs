using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.ChangeClass.Interfaces
{
    public interface ICharacterModel
    {
        public string Id { get; }
        public string Name { get; }
        public int Level { get; }
        public string Element { get; }
        public string ClassId { get; }
        public float Hp { get; }
        public float MaxHp { get; }
        public float Mp { get; }
        public float MaxMp { get; }
        public float Exp { get; }
        public float Str { get; }
        public float Vit { get; }
        public float Agi { get; }
        public float Int { get; }
        public float Luck { get; }
        public float Atk { get; }
        public float MAtk { get; }
        public float Def { get; }
        public float MDef { get; }
    }
}