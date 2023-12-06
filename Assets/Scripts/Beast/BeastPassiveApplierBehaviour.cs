using CryptoQuest.Beast.Interface;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Gameplay.PlayerParty;
using UnityEngine;

namespace CryptoQuest.Beast
{
    public interface IBeastEquippingBehaviour
    {
        public IBeast EquippingBeast { get; set; }
    }

    public class BeastPassiveApplierBehaviour : MonoBehaviour, IBeastEquippingBehaviour
    {
        [SerializeReference] private IBeast _equippingBeast;
        [SerializeField] private PartyManager _party;
        public IBeast EquippingBeast
        {
            get => _equippingBeast;
            set => _equippingBeast = value;
        }
        [SerializeField] private BeastProvider _beastProvider;
        private IBeastPassiveApplier _applier;

        private void Awake()
        {
            _applier = new BeastPassiveApplier(this, _party);
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
            _applier.ApplyPassive(beast);
        }
    }
}
