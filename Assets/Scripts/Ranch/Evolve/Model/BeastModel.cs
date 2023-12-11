using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.Ranch.Evolve.Interface;
using UnityEngine;

namespace CryptoQuest.Ranch.Evolve.Model
{
    public class BeastModel : MonoBehaviour, IBeastModel
    {
        public List<Beast.Beast> Beasts { get; private set; }

        public IEnumerator CoGetData(BeastInventorySO inventory)
        {
            Beasts = new List<Beast.Beast>();
            Beasts.AddRange(inventory.OwnedBeasts);
            yield break;
        }
    }
}