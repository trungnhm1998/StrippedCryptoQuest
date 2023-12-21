﻿using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero;
using UnityEngine;

namespace CryptoQuest.Inventory
{
    public class HeroSpecInitializer : MonoBehaviour
    {
        [SerializeField] private HeroBehaviour _hero;
        public HeroBehaviour HeroBehaviour => _hero;

        public void Init(HeroSpec spec)
        {
            _hero.gameObject.SetActive(true);
            _hero.Spec = spec;
        }

        public void Reset()
        {
            _hero.Spec = new HeroSpec();
            _hero.gameObject.SetActive(false);
        }
    }
}