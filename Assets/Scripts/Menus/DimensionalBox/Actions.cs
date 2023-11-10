using CryptoQuest.Core;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Menus.DimensionalBox
{
    public class GetNftEquipments : ActionBase
    {
        public EEquipmentStatus Status { get; set; } = EEquipmentStatus.All;
        public bool ForceRefresh { get; set; } = false;
    }

    public class GetNftEquipmentsSucceed : ActionBase { }

    public class GetNftEquipmentsFailed : ActionBase { }

    public class TransferSucceed : ActionBase { }
    public class TransferFailed : ActionBase { }
    
    public class GetToken : ActionBase { }

    public class GetTokenSuccess : ActionBase { }

    public class GetTokenFailed : ActionBase { }

    public class TransferringMetad : ActionBase
    {
        public CurrencySO SourceToTransfer { get; private set; }
        public float Amount { get; }

        public TransferringMetad(CurrencySO sourceToTransfer, float amount)
        {
            SourceToTransfer = sourceToTransfer;
            Amount = amount;
        }
    }
    
    public class TransferringMetadSuccess : ActionBase { }
    public class TransferringMetadFailed : ActionBase { }
}