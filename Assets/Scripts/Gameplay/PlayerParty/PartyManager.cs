using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public static class PartyConstants
    {
        public const int MAX_PARTY_SIZE = 4;
    }

    public interface IPartyController
    {
        // bool TryGetMemberAtIndex(int charIndexInParty, out IHero character);
        // IParty Party { get; }
        public PartySlot[] Slots { get; }
        public PartySO PartySO { get; }
        int Size { get; }

        /// <summary>
        /// The order will be setup using sort feature in menu
        /// </summary>
        List<HeroBehaviour> OrderedAliveMembers { get; }

        bool Sort(int sourceIndex, int destinationIndex);
        bool GetHero(int slotIndex, out HeroBehaviour hero);

        void Init();
    }

    public class PartyManager : MonoBehaviour, IPartyController
    {
        [SerializeField, Space] private PartySlot[] _partySlots = new PartySlot[PartyConstants.MAX_PARTY_SIZE];
        public PartySlot[] Slots => _partySlots;

        public int Size => _size;
        private int _size;
        [SerializeField] private PartySO _partySO;
        public PartySO PartySO => _partySO;

        public List<HeroBehaviour> OrderedAliveMembers =>
            (from slot in _partySlots
                where slot.IsValid() && slot.HeroBehaviour.IsValidAndAlive()
                select slot.HeroBehaviour).ToList();

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
        }

        private void Start() => Init();

        private void OnEnable()
        {
            _partySO.Changed += Init;
        }

        private void OnDisable()
        {
            _partySO.Changed -= Init;
        }

        /// <summary>
        /// Init party members stats at run time
        /// and bind the mono behaviour to the <see cref="CharacterSpec._characterComponent"/>
        /// </summary>
        public void Init()
        {
            foreach (var partySlot in _partySlots) partySlot.Reset();
            var heroes = _partySO.GetParty();
            _size = heroes.Length;
            var partySlotIndex = 0;
            for (int i = 0; i < heroes.Length; i++)
            {
                var hero = heroes[i];
                if (hero.IsValid() == false) continue;
                _partySlots[partySlotIndex++].Init(hero);
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

            List<PartySlotSpec> heroes = new List<PartySlotSpec>();
            foreach (var slot in _partySlots)
                if (slot.IsValid())
                    heroes.Add(slot.Spec);

            _partySO.SetParty(heroes.ToArray());
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