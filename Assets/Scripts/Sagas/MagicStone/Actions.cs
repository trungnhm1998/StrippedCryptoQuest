using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Events;

namespace CryptoQuest.Sagas.MagicStone
{
    public class FetchProfileMagicStonesAction : ActionBase { }

    public class ResponseGetMagicStonesFailed : ActionBase { }

    public class ResponseGetMagicStonesSucceeded : ActionBase
    {
        public List<IMagicStone> Stones { get; }

        public ResponseGetMagicStonesSucceeded(List<IMagicStone> stones)
        {
            Stones = stones;
        }
    }

    public abstract class FetchMagicStonesSuccess : ActionBase
    {
        public Objects.MagicStone[] Stones { get; }

        protected FetchMagicStonesSuccess(Objects.MagicStone[] stones)
        {
            Stones = stones;
        }
    }

    public class FetchIngameMagicStonesSuccess : FetchMagicStonesSuccess
    {
        public FetchIngameMagicStonesSuccess(Objects.MagicStone[] stones) : base(stones) { }
    }

    public class FetchInboxMagicStonesSuccess : FetchMagicStonesSuccess
    {
        public FetchInboxMagicStonesSuccess(Objects.MagicStone[] stones) : base(stones) { }
    }

    public class GetStonesResponsed : ActionBase
    {
        public MagicStonesResponse Response { get; }

        public GetStonesResponsed(MagicStonesResponse response)
        {
            Response = response;
        }
    }

    public class GetStonesFailed : ActionBase { }

    public class StoneInventoryFilled : ActionBase { }

    public class TransferMagicStoneFailed : ActionBase { }

    public class TransferMagicStoneSucceed : ActionBase
    {
        public Objects.MagicStone[] ResponseMagicStone { get; }

        public TransferMagicStoneSucceed(Objects.MagicStone[] responseMagicStone)
        {
            ResponseMagicStone = responseMagicStone;
        }
    }
}