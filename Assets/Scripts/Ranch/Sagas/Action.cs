using CryptoQuest.Beast;
using CryptoQuest.Ranch.Object;
using IndiGames.Core.Events;

namespace CryptoQuest.Ranch.Sagas
{
    public class RequestEvolveBeast : ActionBase
    {
        public IBeast Base;
        public IBeast Material;
    }

    public class BeastEvolveRespond : ActionBase
    {
        public EvolveResponse Response { get; private set; }
        public RequestEvolveBeast RequestContext { get; private set; }

        public BeastEvolveRespond(EvolveResponse response, RequestEvolveBeast requestCtx)
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
        public int LevelToUpgrade;
        public IBeast Beast;
    }

    public class BeastUpgradeSucceed : ActionBase { }

    public class BeastUpgradeFailed : ActionBase { }
}