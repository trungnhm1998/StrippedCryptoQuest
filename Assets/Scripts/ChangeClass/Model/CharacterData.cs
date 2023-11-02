using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.ChangeClass.API;
using CryptoQuest.ChangeClass.Interfaces;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.ChangeClass.Models
{
    public class CharacterData : ICharacterModel
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public int Level { get; private set; }
        public string Element { get; private set; }
        public string ClassId { get; private set; }
        public float Hp { get; private set; }
        public float MaxHp { get; private set; }
        public float Mp { get; private set; }
        public float MaxMp { get; private set; }
        public float Exp { get; private set; }
        public float Str { get; private set; }
        public float Vit { get; private set; }
        public float Agi { get; private set; }
        public float Int { get; private set; }
        public float Luck { get; private set; }
        public float Atk { get; private set; }
        public float MAtk { get; private set; }
        public float Def { get; private set; }
        public float MDef { get; private set; }
        public CharacterData(CharacterAPI hero)
        {
            Id = hero.classId;
            Name = hero.name;
            Element = hero.element;
        }
    }
}
