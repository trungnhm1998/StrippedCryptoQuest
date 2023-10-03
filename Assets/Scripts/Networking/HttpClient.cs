using UnityEngine.Networking;
using UniRx;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System.Threading;

namespace CryptoQuest.Networking
{
    public static class HttpClient
    {
        public static IObservable<UnityWebRequest> Get(string url, Dictionary<string, string> headers = null)
        {
            return FetchUrl(url, UnityWebRequest.Get, headers);
        }

        public static IObservable<UnityWebRequest> Post(string url, WWWForm form, Dictionary<string, string> headers = null)
        {
            return FetchUrl(url, uri => UnityWebRequest.Post(uri, form), headers);
        }

        public static IObservable<UnityWebRequest> Post(string url, string bodyData, Dictionary<string, string> headers = null)
        {
            return FetchUrl(url, uri => UnityWebRequest.Post(uri, bodyData, "application/json"), headers);
        }

        public static IObservable<UnityWebRequest> Put(string url, string bodyData, Dictionary<string, string> headers = null)
        {
            return FetchUrl(url, uri => UnityWebRequest.Put(uri, bodyData), headers);
        }

        public static IObservable<UnityWebRequest> Delete(string url, Dictionary<string, string> headers = null)
        {
            return FetchUrl(url, UnityWebRequest.Delete, headers);
        }

        public static IObservable<UnityWebRequest> FetchUrl(string url, Func<string, UnityWebRequest> webRequestFunc, Dictionary<string, string> headers = null)
        {
            return Observable.Create<UnityWebRequest>(observer =>
            {
                var request = webRequestFunc(url);
                var requestDisposable = new SingleAssignmentDisposable();

                //Set default header to accept json
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Accept", "application/json");

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.SetRequestHeader(header.Key, header.Value);
                    }
                }

                IObservable<UnityWebRequest> HandleResult()
                {
                    if (requestDisposable.IsDisposed)
                    {
                        Debug.Log("Login dis");
                        return Observable.Throw<UnityWebRequest>(
                            new OperationCanceledException("Already disposed."));
                    }

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        return Observable.Throw<UnityWebRequest>(new WebException(request.error));
                    }

                    if (request.responseCode != (long)HttpStatusCode.OK)
                    {
                        return Observable.Throw<UnityWebRequest>(
                            new WebException($"{request.responseCode} - {request.downloadHandler.text}"));
                    }

                    return Observable.Return(request);
                }

                requestDisposable.Disposable = request
                    .SendWebRequest()
                    .AsAsyncOperationObservable(new Progress<float>())
                    .ContinueWith(_ => HandleResult())
                    .CatchIgnore((OperationCanceledException _) => observer.OnCompleted())
                    .Subscribe(result =>
                    {
                        observer.OnNext(result);
                        observer.OnCompleted();
                    }, observer.OnError);

                return new CompositeDisposable(request, requestDisposable);
            });
        }
    }
}
