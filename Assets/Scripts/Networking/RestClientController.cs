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

        public IObservable<string> Get(string path, object body = null,
            Dictionary<string, string> headers = null);

        public IObservable<TResponse> Put<TResponse>(string path, object body = null,
            Dictionary<string, string> headers = null);

        public IObservable<string> Put(string path, object body = null,
            Dictionary<string, string> headers = null);

        public IRestClient WithParams(Dictionary<string, string> parameters);
    }

    /// <summary>
    /// Implementation for IRestClient that using <see cref="RestClient"/> plugin
    ///
    /// This doesn't need to be a MonoBehaviour, but it's easier to use it as a MonoBehaviour, otherwise need to inject it somewhere
    /// </summary>
    public class RestClientController : MonoBehaviour, IRestClient
    {
        [SerializeField] private Credentials _credentials;
        [SerializeField] private EnvironmentSO _environment;
        private EnvironmentSO Env => ServiceProvider.GetService<EnvironmentSO>();

        private void Awake()
        {
            ServiceProvider.Provide(_environment);
            ServiceProvider.Provide(_credentials);
            ServiceProvider.Provide<IRestClient>(this);
        }

        public IObservable<TResponse> Post<TResponse>(string path, object body = null,
            Dictionary<string, string> headers = null)
        {
            return Observable.Create<TResponse>(observer =>
            {
                try
                {
                    var generateRequest = GenerateRequest(path, body, headers);
                    PluginRestClient.Post<TResponse>(generateRequest)
                        .Then(observer.OnNext)
                        .Catch(observer.OnError)
                        .Finally(observer.OnCompleted);
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<TResponse> Get<TResponse>(string path, object body = null,
            Dictionary<string, string> headers = null)
        {
            return Observable.Create<TResponse>(observer =>
            {
                try
                {
                    var generateRequest = GenerateRequest(path, body, headers);
                    generateRequest.Method = "GET";
                    PluginRestClient.Get<TResponse>(generateRequest)
                        .Then(observer.OnNext)
                        .Catch(observer.OnError)
                        .Finally(observer.OnCompleted);
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<string> Get(string path, object body = null, Dictionary<string, string> headers = null)
        {
            return Observable.Create<string>(observer =>
            {
                try
                {
                    var generateRequest = GenerateRequest(path, body, headers);
                    generateRequest.Method = "GET";
                    PluginRestClient.Get(generateRequest)
                        .Then(helper => observer.OnNext(helper.Text))
                        .Catch(observer.OnError)
                        .Finally(observer.OnCompleted);
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<TResponse> Put<TResponse>(string path, object body = null,
            Dictionary<string, string> headers = null)
        {
            return Observable.Create<TResponse>(observer =>
            {
                try
                {
                    var generateRequest = GenerateRequest(path, body, headers);
                    PluginRestClient.Put<TResponse>(generateRequest)
                        .Then(observer.OnNext)
                        .Catch(observer.OnError)
                        .Finally(observer.OnCompleted);
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<string> Put(string path, object body = null, Dictionary<string, string> headers = null)
        {
            return Observable.Create<string>(observer =>
            {
                try
                {
                    var generateRequest = GenerateRequest(path, body, headers);
                    PluginRestClient.Put(generateRequest)
                        .Then(helper => observer.OnNext(helper.Text))
                        .Catch(observer.OnError)
                        .Finally(observer.OnCompleted);
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        private Dictionary<string, string> _params;

        public IRestClient WithParams(Dictionary<string, string> parameters)
        {
            _params = parameters;
            return this;
        }

        private RequestHelper GenerateRequest(string path, object body = null,
            Dictionary<string, string> headers = null)
        {
            var mergeHeaders = MergeHeaders(headers);
            var accessToken = _credentials.Profile.token.access.token;
            if (!string.IsNullOrEmpty(accessToken)) mergeHeaders.TryAdd("Authorization", "Bearer " + accessToken);

            var bodyString = "";
            if (body != null) bodyString = JsonConvert.SerializeObject(body);
            var request = new RequestHelper
            {
                Uri = $"{Env.API}/{path}",
                BodyString = bodyString,
                Headers = mergeHeaders,
                Params = _params,
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                EnableDebug = true,
#endif
            };
            
            _params = null;

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