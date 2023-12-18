using CryptoQuest.Beast;
using IndiGames.Core.Events;

namespace CryptoQuest.Ranch.Sagas
{
    public class RequestEvolveBeast : ActionBase
    {
        public IBeast Base;
        public IBeast Material;
    }

    public class EvolveResponsed : ActionBase
    {
        public EvolveResponse Response { get; private set; }
        public RequestEvolveBeast RequestContext { get; private set; }

        public EvolveResponsed(EvolveResponse response, RequestEvolveBeast requestCtx)
        {
            Response = response;
            RequestContext = requestCtx;
        }
    }

    public class EvolveRequestFailed : ActionBase { }

    public class EvolveSucceed : ActionBase { }

    public class EvolveFailed : ActionBase { }

    public class RequestUpgradeBeast : ActionBase
    {
        public int BeforeLevel;
        public IBeast Beast;
    }

    public class UpgradeResponsed : ActionBase
    {
        public UpgradeResponse Response { get; private set; }
        public RequestUpgradeBeast RequestContext { get; private set; }

        public UpgradeResponsed(UpgradeResponse response, RequestUpgradeBeast requestCtx)
        {
            Response = response;
            RequestContext = requestCtx;
        }
    }

    public class UpgradeRequestFailed : ActionBase { }

    public class UpgradeSucceed : ActionBase { }

    public class UpgradeFailed : ActionBase { }
}