using System.Collections.Generic;
using SuperTiled2Unity.Editor;
using UnityEngine;
using ST2USettingsBase = SuperTiled2Unity.Editor.ST2USettings;

namespace CryptoQuestEditor.SuperTiled2Unity
{
    public class ST2USettings : ST2USettingsBase
    {
        [SerializeField]
        private List<TypePrefabReplacement> _customPrefabReplacements = new();

        public List<TypePrefabReplacement> CustomPrefabReplacements => _customPrefabReplacements;
    }
}