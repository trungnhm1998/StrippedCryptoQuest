using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuestClient
{
    [CreateAssetMenu(fileName = "Profile", menuName = "ScriptableObjects/Profile")]
    public class ProfileScriptableObject : ScriptableObject
    {
        public string PlayerName;
    }
}