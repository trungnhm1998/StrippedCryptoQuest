using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Menus.Home.UI
{
    public class UIListItem : MonoBehaviour
    {
        [SerializeField] private Image _classIcon;
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;

        public int Id { get; private set; }

        public void SetInfo(Obj.Character info)
        {
            Id = info.id;
            _name.text = info.name;
            _level.text = $"Lv{info.level}";
            _localizedName.RefreshString();
        }
    }
}