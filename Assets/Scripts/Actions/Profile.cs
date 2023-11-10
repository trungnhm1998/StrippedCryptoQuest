﻿using CryptoQuest.Core;

namespace CryptoQuest.Actions
{
    public class GetProfile : ActionBase { }

    public class GetProfileSucceed : ActionBase { }

    public class GetProfileFailed : ActionBase { }

    public class FetchProfileEquipmentsAction : ActionBase { }

    public class SaveGameAction : ActionBase { }

    public class InventoryFilled : ActionBase { }

    public class UpdateWallet : ActionBase
    {
        public int Gold { get; }
        public int Soul { get; }
        public int Diamond { get; }
        public string WalletAddress { get; }

        public UpdateWallet(int gold, int soul, int diamond, string walletAddress)
        {
            Gold = gold;
            Soul = soul;
            Diamond = diamond;
            WalletAddress = walletAddress;
        }
    }
}