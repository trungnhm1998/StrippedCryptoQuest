using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public interface IPartyController
    {
        // bool TryGetMemberAtIndex(int charIndexInParty, out IHero character);
        // IParty Party { get; }
        public PartySlot[] Slots { get; }
        int Size { get; }
        bool Sort(int sourceIndex, int destinationIndex);
        bool GetHero(int slotIndex, out HeroBehaviour hero);
    }

    public class PartyManager : MonoBehaviour, IPartyController
    {
        [SerializeField, Space] private PartySlot[] _partySlots = new PartySlot[PartyConstants.MAX_PARTY_SIZE];
        public PartySlot[] Slots => _partySlots;

        public int Size => _size;
        private int _size;
        private IPartyProvider _partyProvider;

        private void OnValidate()
        {
            if (_partySlots.Length != PartyConstants.MAX_PARTY_SIZE)
            {
                Array.Resize(ref _partySlots, PartyConstants.MAX_PARTY_SIZE);
            }

            _partySlots = GetComponentsInChildren<PartySlot>();
        }

        private void Awake()
        {
            ServiceProvider.Provide<IPartyController>(this);
            _partyProvider = GetComponent<IPartyProvider>();
            InitParty();
        }

        /// <summary>
        /// Init party members stats at run time
        /// and bind the mono behaviour to the <see cref="CharacterSpec._characterComponent"/>
        /// </summary>
        private void InitParty()
        {
            var heroes = _partyProvider.GetParty();
            _size = heroes.Length;
            for (int i = 0; i < heroes.Length; i++)
            {
                var hero = heroes[i];
                _partySlots[i].Init(hero);
            }
        }

        /// <summary>
        /// Despite the name, it's actually swapping the members
        /// Cannot sort into empty slot
        /// Both slot must be valid
        /// </summary>
        public bool Sort(int sourceIndex, int destinationIndex)
        {
            if (sourceIndex < 0 || sourceIndex >= _size)
            {
                Debug.LogError("PartySO::Sort::Invalid source index");
                return false;
            }

            if (destinationIndex < 0 || destinationIndex >= _size)
            {
                Debug.LogError("PartySO::Sort::Invalid destination index");
                return false;
            }

            if (sourceIndex == destinationIndex)
            {
                Debug.LogWarning("PartyS::Sort::Source is the same as destination index");
                return false;
            }

            var destMember = _partySlots[destinationIndex];
            if (destMember == null || destMember.IsValid() == false)
            {
                Debug.LogError("PartySO::Sort::Cannot sort into empty slot");
                return false;
            }

            var memberToSort = _partySlots[sourceIndex];
            if (memberToSort == null || memberToSort.IsValid() == false)
            {
                Debug.LogError("PartySO::Sort::Invalid source or destination index");
                return false;
            }

            // Either this destructuring or 3 lines of code
            (_partySlots[sourceIndex], _partySlots[destinationIndex]) =
                (_partySlots[destinationIndex], _partySlots[sourceIndex]);

            Debug.Log($"Sorted {sourceIndex} to {destinationIndex}");

            List<HeroSpec> heroes = new List<HeroSpec>();
            foreach (var slot in _partySlots)
                if (slot.IsValid()) heroes.Add(slot.HeroBehaviour.Spec);
            
            _partyProvider.SetParty(heroes.ToArray());

            return true;
        }

        public bool GetHero(int slotIndex, out HeroBehaviour hero)
        {
            hero = null;
            if (slotIndex < 0 || slotIndex >= _size)
            {
                Debug.LogError("PartySO::GetHero::Invalid slot index");
                return false;
            }

            var partySlot = _partySlots[slotIndex];
            if (partySlot.IsValid() == false)
            {
                Debug.LogError($"PartySO::GetHero::Slot {slotIndex} is empty");
                return false;
            }

            hero = partySlot.HeroBehaviour;
            return true;
        }
    }
}