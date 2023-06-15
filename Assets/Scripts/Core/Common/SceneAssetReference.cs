using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace CryptoQuest.Core.Common
{
    [Serializable]
    public class SceneAssetReference : AssetReference
    {
        public SceneAssetReference(string guid) : base(guid)
        {
        }

        public override bool ValidateAsset(Object obj)
        {
            // log obj type
            Debug.Log(obj.GetType());
            return base.ValidateAsset(obj);
        }

        public override bool ValidateAsset(string path)
        {
            return path.EndsWith(".unity");
        }
    }
}