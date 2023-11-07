using CryptoQuest.Core;
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
}