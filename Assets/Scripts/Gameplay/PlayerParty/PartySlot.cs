using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    /// <summary>
    /// I do not want to re-initialize the character when the party is sorted, Simply change the parent of the character
    /// to another slot.
    /// </summary>
    public class PartySlot : MonoBehaviour
    {
        private HeroBehaviour _hero;
        public HeroBehaviour HeroBehaviour => _hero;

        private void Awake() => _hero = GetComponentInChildren<HeroBehaviour>();

        public void Init(HeroSpec hero) => _hero.Init(hero);

        public bool IsValid() => _hero != null && _hero.IsValid();
    }
}