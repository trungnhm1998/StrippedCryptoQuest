using System.Collections.Generic;
using CryptoQuest.BlackSmith.UpgradeStone.Sagas;
using CryptoQuest.Item.MagicStone;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class UpgradeStoneModel : MonoBehaviour
    {
        public Dictionary<PreviewMappingKey, IMagicStone> _previewMapping { get; private set; } = new();
        private TinyMessageSubscriptionToken _previewSuccessToken;

        private void OnEnable()
        {
            _previewSuccessToken = ActionDispatcher.Bind<UpgradePreviewSuccess>(OnPreviewSuccess);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_previewSuccessToken);
        }

        private void OnPreviewSuccess(UpgradePreviewSuccess obj)
        {
            _previewMapping.TryAdd(new PreviewMappingKey
            {
                ElementId = int.Parse(obj.Stone.Definition.ID),
                Level = obj.Stone.Level - 1
            }, obj.Stone);
        }


        public IMagicStone GetUpgradedStone(IMagicStone stoneToUpgrade)
        {
            return TryGetPreview(stoneToUpgrade, out var preview) ? preview : null;
        }

        public bool TryGetPreview(IMagicStone stone, out IMagicStone preview)
        {
            preview = null;
            foreach (var item in _previewMapping)
            {
                if (item.Key.ElementId != int.Parse(stone.Definition.ID) || item.Key.Level != stone.Level) continue;
                preview = item.Value;
                return true;
            }

            return false;
        }
    }

    public class PreviewMappingKey
    {
        public int ElementId { get; set; }
        public int Level { get; set; }
    }
}