using CryptoQuest.Battle.Components;
using IndiGames.Core.Events;
using TinyMessenger;
using TMPro;
using UnityEngine;

namespace CryptoQuest.UI.Tooltips.Equipment
{
    public class RequestInspectHeroAction : ActionBase
    {
        public RequestInspectHeroAction() { }
    } 

    public class InspectHeroAction : ActionBase
    {
        public HeroBehaviour Hero { get; private set; }

        public InspectHeroAction(HeroBehaviour hero) => Hero = hero;
    } 

    public class UIRequiredLv : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Color _originalColor = Color.white;
        [SerializeField] private Color _levelNotEnoughColor = Color.red;
        public int RequiredLevel { get; private set; } = 1;

        private TinyMessageSubscriptionToken _inspectHeroActionToken;


        private void Awake()
        {
            _inspectHeroActionToken = ActionDispatcher.Bind<InspectHeroAction>(HandleInspectHero);
            ActionDispatcher.Dispatch(new RequestInspectHeroAction());
        }
        
        private void OnDestroy()
        {
            ActionDispatcher.Unbind(_inspectHeroActionToken);
        }

        public void SetRequiredLevel(int requiredLevel)
        {
            RequiredLevel = requiredLevel;
            _text.color = _originalColor;
            ActionDispatcher.Dispatch(new RequestInspectHeroAction());
        }

        private void HandleInspectHero(InspectHeroAction ctx)
        {
            var inspectingHero = ctx.Hero;
            if (inspectingHero == null || !inspectingHero.IsValid()) 
                return;

            inspectingHero.TryGetComponent(out LevelSystem levelSystem);
            var charLvl = levelSystem.Level;
            _text.color = charLvl >= RequiredLevel ? _originalColor : _levelNotEnoughColor;
        }
    }
}