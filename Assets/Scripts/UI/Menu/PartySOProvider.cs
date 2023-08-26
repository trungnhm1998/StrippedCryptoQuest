using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.PlayerParty;
using UnityEngine;

namespace CryptoQuest.UI.Menu
{
    /// <summary>
    /// Act as a provider for the party information, Menu will not directly interact with PartySO
    /// </summary>
    public class PartySOProvider : MonoBehaviour, IParty
    {
        [SerializeField] private PartySO _party;
        public CharacterSpec[] Members => _party.Members;

        public bool Sort(int sourceIndex, int destinationIndex)
        {
            return _party.Sort(sourceIndex, destinationIndex);
        }
    }
}