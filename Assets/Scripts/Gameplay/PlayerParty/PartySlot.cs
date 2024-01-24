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
        public HeroBehaviour HeroBehaviour => _hero ??= GetComponentInChildren<HeroBehaviour>();
        public PartySlotSpec Spec { get; private set; }

        private void Awake()
        {
            Reset();
        }

        public void Reset()
        {
            HeroBehaviour.gameObject.SetActive(false);
            HeroBehaviour.Spec = new HeroSpec();
        }

        public void Init(PartySlotSpec slotSpec)
        {
            Reset();
            Spec = slotSpec;
            HeroBehaviour.gameObject.SetActive(true);
            HeroBehaviour.Init(slotSpec);
        }

        public bool IsValid() => HeroBehaviour != null && HeroBehaviour.IsValid();
    }
}