using UnityEngine;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Beast.Interface;

namespace CryptoQuest.Beast
{
    public interface IBeastEquippingBehaviour
    {
        public IBeast EquippingBeast { get; set; }
    }

    public class BeastPassiveControllerBehaviour : MonoBehaviour, IBeastEquippingBehaviour
    {
        [SerializeReference] private IBeast _equippingBeast;
        [SerializeField] private PartyManager _party;
        public IBeast EquippingBeast
        {
            get => _equippingBeast;
            set => _equippingBeast = value;
        }
        [SerializeField] private BeastProvider _beastProvider;
        private IBeastPassiveController _controller;

        private void Awake()
        {
            BeastPassiveController controller = new(this, _party);
            _controller = controller;
        }

        private void OnEnable()
        {
            _beastProvider.EquippingBeastChanged += ApplyPassive;
        }

        private void OnDisable()
        {
            _beastProvider.EquippingBeastChanged -= ApplyPassive;
        }

        private void ApplyPassive(IBeast beast)
        {
            _controller.ApplyPassive(beast);
        }
    }
}
