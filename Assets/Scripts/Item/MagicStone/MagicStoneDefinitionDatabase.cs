using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Sagas.MagicStone;
using UnityEngine;

namespace CryptoQuest.Item.MagicStone
{
    public interface IMagicStoneDefDatabase
    {
        MagicStoneDef this[string id] { get; }
    }

    public class MagicStoneDefinitionDatabase : ScriptableObject, IMagicStoneDefDatabase
    {
        [SerializeField] private MagicStoneDef[] _defs;

        private Dictionary<string, MagicStoneDef> _lookupTable = new();

        private void OnEnable()
        {
            _lookupTable = _defs.ToDictionary(d => d.ID);
        }

        public MagicStoneDef this[string id] => _lookupTable[id];
    }
}