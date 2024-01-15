using System.Collections.Generic;
using CryptoQuest.System.SaveSystem;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    /// <summary>
    /// Save order and character order
    /// </summary>
    public class PartySaver : MonoBehaviour
    {
        [SerializeField] private SaveSystemSO _saveSystemSO;
        [SerializeField] private PartySO _partySO;

        private void OnEnable()
        {
            _partySO.Changed += SavePartyOrder;
        }

        private void OnDisable()
        {
            _partySO.Changed -= SavePartyOrder;
        }

        private void SavePartyOrder()
        {
            List<int> ids = new();
            foreach (var partySlotSpec in _partySO.GetParty())
            {
                ids.Add(partySlotSpec.Hero.Id);
            }

            _saveSystemSO.SaveData[_partySO.name] = JsonUtility.ToJson(new PartyOrderSerializeObject()
            {
                Ids = ids.ToArray()
            });
            _saveSystemSO.Save();
        }
    }
}