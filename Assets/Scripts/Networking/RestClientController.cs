using System;
using System.Collections.Generic;
using CryptoQuest.Environment;
using CryptoQuest.System;
using Newtonsoft.Json;
using Proyecto26;
using UniRx;
using UnityEngine;
using PluginRestClient = Proyecto26.RestClient;

namespace CryptoQuest.Networking
{
    public interface IRestClient
    {
        public IObservable<TResponse> Post<TResponse>(string path, object body = null,
            Dictionary<string, string> headers = null);

        public IObservable<TResponse> Get<TResponse>(string path, object body = null,
            Dictionary<string, string> headers = null);
    }

    /// <summary>
    /// Implementation for IRestClient that using <see cref="RestClient"/> plugin
    ///
    /// This doesn't need to be a MonoBehaviour, but it's easier to use it as a MonoBehaviour, otherwise need to inject it somewhere
    /// </summary>
    public class RestClientController : MonoBehaviour, IRestClient
    {
        private EnvironmentSO Env => ServiceProvider.GetService<EnvironmentSO>();
        private void Awake() => ServiceProvider.Provide<IRestClient>(this);

        public IObservable<TResponse> Post<TResponse>(string path, object body = null,
            Dictionary<string, string> headers = null)
        {
            return Observable.Create<TResponse>(observer =>
            {
                var generateRequest = GenerateRequest(path, body, headers);
                PluginRestClient.Post<TResponse>(generateRequest)
                    .Then(observer.OnNext)
                    .Catch(observer.OnError)
                    .Finally(observer.OnCompleted);

                return Disposable.Empty;
            });
        }

        public IObservable<TResponse> Get<TResponse>(string path, object body = null,
            Dictionary<string, string> headers = null)
        {
            return Observable.Create<TResponse>(observer =>
            {
                var generateRequest = GenerateRequest(path, body, headers);
                PluginRestClient.Get<TResponse>(generateRequest)
                    .Then(observer.OnNext)
                    .Catch(observer.OnError)
                    .Finally(observer.OnCompleted);

                return Disposable.Empty;
            });
        }

        private RequestHelper GenerateRequest(string path, object body = null,
            Dictionary<string, string> headers = null)
        {
            var request = new RequestHelper
            {
                Uri = $"{Env.API}{path}",
                BodyString = JsonConvert.SerializeObject(body),
                Headers = MergeHeaders(headers),
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                EnableDebug = true,
#endif
            };

            return request;
        }

        private static Dictionary<string, string> MergeHeaders(Dictionary<string, string> headers)
        {
            var defaultHeaders = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Accept", "application/json" },
            };

            var finalHeaders = new Dictionary<string, string>();
            foreach (var header in defaultHeaders)
            {
                finalHeaders.TryAdd(header.Key, header.Value);
            }

            if (headers == null) return finalHeaders;
            foreach (var header in headers)
            {
                finalHeaders.TryAdd(header.Key, header.Value);
            }

            return finalHeaders;
        }
    }
}