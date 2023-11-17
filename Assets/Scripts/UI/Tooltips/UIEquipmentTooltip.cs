using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Tooltips
{
    public class UIEquipmentTooltip : MonoBehaviour
    {
        [SerializeField] private Image _headerBackground;
        [SerializeField] private Image _rarity;
        [SerializeField] private GameObject _nftTag;
        [SerializeField] private Image _illustration;
        [SerializeField] private LocalizeStringEvent _nameLocalize;

        private RectTransform _rectTransform;
        private EquipmentTooltipContext _context;
        private EquipmentInfo Equipment => _context?.Equipment;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            // rebuild layout to get the correct size
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }

        public void Init(EquipmentTooltipContext context)
        {
            _context = context;
            SetupInfo();
            SetupPosition();
            SetupIllustration();
        }

        private void SetupInfo()
        {
            _illustration.enabled = false;
            _headerBackground.color = Equipment.Rarity.Color;
            _rarity.sprite = Equipment.Rarity.Icon;
            _nftTag.SetActive(Equipment.IsNftItem);
            _nameLocalize.StringReference = Equipment.DisplayName;
        }

        private void SetupPosition()
        {
            SetPivot();
            SetPositionAtSelectedGameObject();
        }

        private void SetPivot()
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            var rectTransform = selectedGameObject.GetComponent<RectTransform>();

            var position = rectTransform.position;
            var pivotX = position.x / TooltipSafeArea.Width;
            var pivotY = position.y / TooltipSafeArea.Height;
            _rectTransform.pivot = new Vector2(0, 1);
        }

        private void SetPositionAtSelectedGameObject()
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null) return;
            var rectTransform = selectedGameObject.GetComponent<RectTransform>();
            var xPos = _context.Position.x;
            var yPos = _context.Position.y - rectTransform.rect.yMax;
            _rectTransform.position = new Vector2(xPos, yPos);
        }

        private void SetupIllustration()
        {
            _illustration.LoadSpriteAndSet(Equipment.Config.Image);
        }

        private void OnDisable()
        {
            if (Equipment == null ||
                Equipment.Config == null)
                return;
            Equipment.Config.Image.ReleaseAsset();
        }

        private void OnGUI()
        {
            var corners = new Vector3[4];
            _rectTransform.GetWorldCorners(corners);
            var labelWidth = 200;
            var labelHeight = 30;

            // label style with white background
            var labelStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = Texture2D.whiteTexture, textColor = Color.black },
                alignment = TextAnchor.MiddleCenter
            };

            foreach (var corner in corners)
            {
                var v2 = new Vector2(corner.x, corner.y);
                if (v2.x + labelWidth > corners[2].x) v2.x = corners[2].x - labelWidth;
                if (v2.y - labelHeight < corners[0].y) v2.y = corners[0].y + labelHeight;
                GUI.Label(new Rect(v2.x, Screen.height - v2.y, labelWidth, labelHeight), $"Corner: {v2}", labelStyle);
            }
        }
    }
}