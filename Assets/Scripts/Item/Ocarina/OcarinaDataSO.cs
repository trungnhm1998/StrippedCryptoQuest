using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest
{
    [CreateAssetMenu(menuName = "Crypto Quest/Item/Ocarina Data SO")]
    public class OcarinaDataSO : ScriptableObject
    {
        public List<OcarinaData> ocarinaDataList;
        public List<SceneScriptableObject> ocarinaBlockSceneList;
    }
}