using CryptoQuest.UI.Character;
using UnityEngine;

namespace CryptoQuest.Menus.Status.UI.Stats
{
    public class UIStats : MonoBehaviour
    {
        [Header("UI references")]
        [SerializeField] private UIAttributeBar _hp;
        [SerializeField] private UIAttributeBar _mp;
        [SerializeField] private UIAttributeBar _exp;
    }
}