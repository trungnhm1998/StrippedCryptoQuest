using System;
using System.Collections.Generic;
using CryptoQuest.Environment;
using CryptoQuest.Networking.API;
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
        public IObservable<TResponse> Post<TResponse>(string path);

        public IObservable<TResponse> Get<TResponse>(string path);

        public IObservable<string> Get(string path);

        public IObservable<TResponse> Put<TResponse>(string path);

        public IObservable<string> Put(string path);

        public IRestClient WithParams(Dictionary<string, string> parameters);
        public IRestClient WithBody(object body);
        public IRestClient WithHeaders(Dictionary<string, string> headers);
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

        public IObservable<TResponse> Post<TResponse>(string path)
        {
            return Observable.Create<TResponse>(observer =>
            {
                try
                {
                    var generateRequest = GenerateRequest(path);
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

        public IObservable<TResponse> Get<TResponse>(string path)
        {
            return Observable.Create<TResponse>(observer =>
            {
                try
                {
                    var generateRequest = GenerateRequest(path);
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

        public IObservable<string> Get(string path)
        {
            return Observable.Create<string>(observer =>
            {
                try
                {
                    var generateRequest = GenerateRequest(path);
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

        public IObservable<TResponse> Put<TResponse>(string path)
        {
            return Observable.Create<TResponse>(observer =>
            {
                try
                {
                    var generateRequest = GenerateRequest(path);
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

        public IObservable<string> Put(string path)
        {
            return Observable.Create<string>(observer =>
            {
                try
                {
                    var generateRequest = GenerateRequest(path);
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

        private object _body;
        public IRestClient WithBody(object body)
        {
            _body = body;
            return this;
        }

        private Dictionary<string, string> _headers;
        public IRestClient WithHeaders(Dictionary<string, string> headers)
        {
            _headers = headers;
            return this;
        }

        private RequestHelper GenerateRequest(string path)
        {
            var mergeHeaders = MergeHeaders(_headers);
            var accessToken = _credentials?.Profile?.token?.access?.token;

            // Add authorization only when this is not login/refresh requests
            if (!path.Contains(Accounts.LOGIN)
                && !path.Contains(Accounts.DEBUG_LOGIN) 
                && !path.Contains(Accounts.REFRESH_TOKENS) 
                && !string.IsNullOrEmpty(accessToken)) 
            { 
                mergeHeaders.TryAdd("Authorization", "Bearer " + accessToken); 
            }

            string bodyString = null;
            if (_body != null)
            {
                bodyString = JsonConvert.SerializeObject(_body);
            }

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
            _body = null;
            _headers = null;

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