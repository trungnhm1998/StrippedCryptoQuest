using System;
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
            throw new NotImplementedException();
        }

        private void OnDisable()
        {
            throw new NotImplementedException();
        }
    }
}