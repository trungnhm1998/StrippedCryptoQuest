using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle.CommandsMenu
{
    public interface IAutoScrollViewCalculator
    {
        float CalculateNormalizedScrollPosition(ScrollRect scrollRect, RectTransform targetRect, float align = 0);
    }
}