using IndiGames.Core.Events;
using System;
using CryptoQuest.Gameplay.Inventory;

namespace CryptoQuest.Sagas
{
    [Serializable]
    public class CommonResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public float diamond;
        public int soul;
        public long time;
    }

    public class UpdateWalletAction : ActionBase
    {
        public CommonResponse Response;

        public UpdateWalletAction(CommonResponse response)
        {
            Response = response;
        }
    }

    public class UpdateWalletSaga : SagaBase<UpdateWalletAction>
    {
        protected override void HandleAction(UpdateWalletAction ctx)
        {
            ActionDispatcher.Dispatch(new SetGoldAction(ctx.Response.gold));
            ActionDispatcher.Dispatch(new SetDiamondAction(ctx.Response.diamond));
            ActionDispatcher.Dispatch(new SetSoulAction(ctx.Response.soul));
        }
    }
}