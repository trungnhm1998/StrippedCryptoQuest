using CryptoQuest.Beast;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Ranch.UI
{
    public class UIBeastItem : MonoBehaviour
    {
        public static event UnityAction<UIBeastItem> Pressed;

        [SerializeField] private Image _beatIcon;
        [SerializeField] private LocalizeStringEvent _localizeName;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private GameObject _pendingTag;
        [SerializeField] private GameObject _inGameTag;
        [SerializeField] private GameObject _transferringTag;
        [SerializeField] private Button _button;
        private IBeast _beast;
        public IBeast Beast => _beast;
        public BeastResponse Response { get; private set; }
        public int Id => Response.id;

        private void OnDisable() => _beast = NullBeast.Instance;

        public bool MarkedForTransfer
        {
            get => _pendingTag.activeSelf;
            set => _pendingTag.SetActive(value);
        }

        public void Initialize(BeastResponse beast)
        {
            _transferringTag.SetActive(beast.IsTransferring);
            _beast = NullBeast.Instance;
            MarkedForTransfer = false;
            Response = beast;

            _beast = ServiceProvider.GetService<IBeastResponseConverter>().Convert(beast);
            _name.text = $"{Id}.{Response.name}";
            _level.text = $"Lv.{_beast.Level}";
        }

        public void OnSelectToTransfer()
        {
            if (_inGameTag.activeSelf || _transferringTag.activeSelf) return;
            MarkedForTransfer = !MarkedForTransfer;
            Pressed?.Invoke(this);
        }
    }
}