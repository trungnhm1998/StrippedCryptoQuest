using System;
using System.Net;
using CryptoQuest.Networking;
using IndiGames.Core.Events;
using Proyecto26;

namespace CryptoQuest.UI.Popups.Sagas
{
    public class ResponseWithErrorSaga : SagaBase<ResponseWithError>
    {
        protected override void HandleAction(ResponseWithError ctx)
        {
            var exception = ctx.RequestException;

            if (exception is AggregateException aggregateException)
            {
                exception = aggregateException.InnerException;
            }

            if (exception is not RequestException requestException)
            {
                ActionDispatcher.Dispatch(new ClientErrorPopup());
                return;
            }

            if (requestException.StatusCode == (long)HttpStatusCode.ServiceUnavailable)
            {
                ActionDispatcher.Dispatch(new ServerMaintaining(requestException));
                return;
            }

            if (requestException.StatusCode == (long)HttpStatusCode.Unauthorized)
            {
                ActionDispatcher.Dispatch(new UnauthorizedPopup());
                return;
            }

            ActionDispatcher.Dispatch(new ServerErrorPopup());
        }
    }
}