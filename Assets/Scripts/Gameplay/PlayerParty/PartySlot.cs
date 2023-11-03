using CryptoQuest.Battle.Components;
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
        public PartySlotSpec Spec { get; private set; }

        private void Awake()
        {
            _hero = GetComponentInChildren<HeroBehaviour>();
            _hero.gameObject.SetActive(false);
        }

        public void Init(PartySlotSpec slotSpec)
        {
            Spec = slotSpec;
            _hero.gameObject.SetActive(true);
            _hero.Init(slotSpec);
        }

        public bool IsValid() => _hero != null && _hero.IsValid();
    }
}