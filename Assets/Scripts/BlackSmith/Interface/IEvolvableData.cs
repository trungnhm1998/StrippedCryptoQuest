using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Interface
{
    public interface IEvolvableData
    {
        Sprite Icon { get; }
        LocalizedString LocalizedName { get; }
        int Level { get; }
        int Stars { get; }
        int Gold { get; }
        float Metad { get; }
    }
}