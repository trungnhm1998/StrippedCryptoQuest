using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Events;

namespace CryptoQuest.Menus.DimensionalBox
{
    public class FetchNftEquipments : ActionBase { }

    public abstract class FetchEquipmentsSuccess : ActionBase
    {
        public EquipmentResponse[] Equipments { get; }

        protected FetchEquipmentsSuccess(EquipmentResponse[] equipments)
        {
            Equipments = equipments;
        }
    }

    public class FetchNftEquipmentSucceed : FetchEquipmentsSuccess
    {
        public FetchNftEquipmentSucceed(EquipmentResponse[] equipments) : base(equipments) { }
    }

    public class FetchIngameEquipmentsSuccess : FetchEquipmentsSuccess
    {
        public FetchIngameEquipmentsSuccess(EquipmentResponse[] equipments) : base(equipments) { }
    }

    public class FetchInboxEquipmentsSuccess : FetchEquipmentsSuccess
    {
        public FetchInboxEquipmentsSuccess(EquipmentResponse[] equipments) : base(equipments) { }
    }

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

    public class GetNftMagicStoneSucceed : ActionBase { }

    public class GetNftMagicStoneFailed : ActionBase { }

    public class GetNftMagicStone : ActionBase
    {
        public EMagicStoneStatus Status { get; set; } = EMagicStoneStatus.All;
        public bool ForceRefresh { get; set; } = false;
    }

    public class SendMagicStoneToBothSide : ActionBase
    {
        public int[] SelectedInDboxMagicStones { get; }
        public int[] SelectedInGameMagicStones { get; }

        public SendMagicStoneToBothSide(int[] selectedInGameMagicStones, int[] selectedInDboxMagicStones)
        {
            SelectedInGameMagicStones = selectedInGameMagicStones;
            SelectedInDboxMagicStones = selectedInDboxMagicStones;
        }
    }

    public class TransferMagicStoneFailed : ActionBase { }

    public class TransferMagicStoneSucceed : ActionBase
    {
        public MagicStone[] ResponseMagicStone { get; }

        public TransferMagicStoneSucceed(MagicStone[] responseMagicStone)
        {
            ResponseMagicStone = responseMagicStone;
        }
    }
}