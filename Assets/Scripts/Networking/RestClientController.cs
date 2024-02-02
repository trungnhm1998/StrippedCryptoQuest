using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using CryptoQuest.API;
using CryptoQuest.Environment;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Proyecto26;
using UniRx;
using UnityEngine;
using static Newtonsoft.Json.JsonConvert;
using PluginRestClient = Proyecto26.RestClient;

namespace CryptoQuest.Networking
{
    public enum ERequestMethod
    {
        GET = 0,
        POST = 1,
        PUT = 2,
        DELETE = 3,
    }

    public interface IRestClient
    {
        public IObservable<TResponse> Post<TResponse>(string path);
        public IObservable<TResponse> Get<TResponse>(string path);
        public IObservable<TResponse> Put<TResponse>(string path);
        public IObservable<TResponse> Request<TResponse>(ERequestMethod method, string path);
        public IRestClient WithParam(string key, string value);
        public IRestClient WithParams(Dictionary<string, string> parameters);
        public IRestClient WithBody(object body);
        public IRestClient WithHeaders(Dictionary<string, string> headers);
        public IRestClient WithoutDispactError();
    }

    public class ResponseWithError : ActionBase
    {
        public Exception RequestException { get; }

        public ResponseWithError(Exception requestException)
        {
            RequestException = requestException;
        }
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

        private void Awake() => ServiceProvider.Provide<IRestClient>(this);

        public IObservable<TResponse> Post<TResponse>(string path)
        {
            return CreateRequest<TResponse>(observer =>
            {
                var generateRequest = GenerateRequest(path);
                PluginRestClient.Post<TResponse>(generateRequest)
                    .Then(observer.OnNext)
                    .Catch(observer.OnError)
                    .Finally(observer.OnCompleted);
            });
        }

        public IObservable<TResponse> Get<TResponse>(string path)
        {
            return CreateRequest<TResponse>(observer =>
            {
                var generateRequest = GenerateRequest(path);
                generateRequest.Method = "GET";
                PluginRestClient.Get<TResponse>(generateRequest)
                    .Then(observer.OnNext)
                    .Catch(observer.OnError)
                    .Finally(observer.OnCompleted);
            });
        }

        public IObservable<TResponse> Put<TResponse>(string path)
        {
            return CreateRequest<TResponse>(observer =>
            {
                var generateRequest = GenerateRequest(path);
                PluginRestClient.Put<TResponse>(generateRequest)
                    .Then(observer.OnNext)
                    .Catch(observer.OnError)
                    .Finally(observer.OnCompleted);
            });
        }

        public IObservable<TResponse> Request<TResponse>(ERequestMethod method, string path)
        {
            return CreateRequest<TResponse>(observer =>
            {
                var generateRequest = GenerateRequest(path);
                generateRequest.Method = method.ToString();

                PluginRestClient.Request<TResponse>(generateRequest)
                    .Then(observer.OnNext)
                    .Catch(observer.OnError)
                    .Finally(observer.OnCompleted);
            });
        }

        private IObservable<TResponse> CreateRequest<TResponse>(Action<ObserverWrapper<TResponse>> handleRequest)
        {
            return Observable.Create<TResponse>(observer =>
            {
                var wrapper = new ObserverWrapper<TResponse>()
                {
                    Observer = observer,
                    IsDispactError = _isDispactError
                };
                _isDispactError = true;

                try
                {
                    handleRequest?.Invoke(wrapper);
                }
                catch (Exception e)
                {
                    wrapper.HandleError(e);
                }

                return Disposable.Empty;
            });
        }


        public IRestClient WithParam(string key, string value)
        {
            _params ??= new Dictionary<string, string>();
            _params.TryAdd(key, value);
            return this;
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

        private bool _isDispactError = true;

        public IRestClient WithoutDispactError()
        {
            _isDispactError = false;
            return this;
        }

        private RequestHelper GenerateRequest(string path)
        {
            var mergeHeaders = MergeHeaders(_headers);
            var accessToken = _credentials.Token;

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
                bodyString = SerializeObject(_body);
            }

            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            mergeHeaders.TryAdd("x-signature", GetHash($"{bodyString}_{timestamp}", _environment.PKEY));
            mergeHeaders.TryAdd("x-timestamp", timestamp.ToString());

            var request = new RequestHelper
            {
                Uri = $"{_environment.API}/{path}",
                BodyString = bodyString,
                Headers = mergeHeaders,
                Params = _params,
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                EnableDebug = true,
#endif
            };


#if DEVELOPMENT_BUILD || UNITY_EDITOR
            if (!string.IsNullOrEmpty(bodyString))
                Debug.Log(
                    $"<color=white>RestClientController::GenerateRequest::Request With Body</color>:: {bodyString}");
            Debug.Log(
                $"<color=white>RestClientController::GenerateRequest::Request With Header</color>:: {SerializeObject(mergeHeaders)}");
#endif
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


        public static string GetHash(string text, string key)
        {
            // change according to your needs, an UTF8Encoding
            // could be more suitable in certain situations
            ASCIIEncoding encoding = new ASCIIEncoding();

            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);

            Byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    /// <summary>
    /// Wrap observer to optional handle error
    /// </summary>
    public class ObserverWrapper<TResponse> : IObserver<TResponse>
    {
        public IObserver<TResponse> Observer;
        public bool IsDispactError;

        public void OnCompleted() => Observer.OnCompleted();
        public void OnError(Exception error) => HandleError(error);
        public void OnNext(TResponse value) => Observer.OnNext(value);

        public void HandleError(Exception exception)
        {
            if (IsDispactError)
                ActionDispatcher.Dispatch(new ResponseWithError(exception));

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogWarning(exception);
#endif
            Observer.OnError(exception);
        }
    }
}