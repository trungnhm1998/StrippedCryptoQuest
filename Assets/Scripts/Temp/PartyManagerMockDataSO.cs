using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.MockData
{
    [CreateAssetMenu(menuName = "Gameplay/MockData/Party Manager")]
    public class PartyManagerMockDataSO : ScriptableObject
    {
        public List<CharInfoMockDataSO> Members;
    }
}
