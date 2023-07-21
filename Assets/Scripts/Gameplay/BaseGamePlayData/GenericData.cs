using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace CryptoQuest
{
    [CreateAssetMenu(fileName = "GenericData", menuName = "Gameplay/BaseGameplayData/GenericData")]
    public class GenericData : ScriptableObject
    {
        public int Id;
        public LocalizedString Name;
        public LocalizedString Description;
    }
}