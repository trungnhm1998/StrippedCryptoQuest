using CryptoQuest.Beast;
using CryptoQuest.Ranch.UI;
using UnityEngine;

namespace CryptoQuest.Ranch.Tooltip
{
    public class TooltipBeastProvider : MonoBehaviour, ITooltipBeastProvider
    {
        [SerializeField] private UIBeastItem _uiBeast;
        public IBeast Beast => _uiBeast.Beast;
    }
}