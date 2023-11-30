using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace CryptoQuest.UI.Extensions
{
    public static class AssetReferenceExtensions
    {
        public static IEnumerator LoadSpriteAndSet(this AssetReferenceT<Sprite> illustration, Image image)
        {
            if (!illustration.OperationHandle.IsValid())
            {
                if (!illustration.RuntimeKeyIsValid()) yield break;
                var handle = illustration.LoadAssetAsync<Sprite>();
                yield return handle;
                image.sprite = handle.Result;
            }
            else
                image.sprite = (Sprite)illustration.OperationHandle.Result;
        }
    }
}