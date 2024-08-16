using RestSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Abstract;
using Microsoft.Extensions.Logging;
using Amazon.Runtime;
using ValueBlue.Core.Entities.Concrete;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ValueBlue.Core.REST.RestSharp
{
    public class RestSharpHttpRepositoryBase : IHttpRepository
    {
        protected readonly RestClient restClient;
        protected readonly ILogger logger;

        public RestSharpHttpRepositoryBase(string baseUrl, ILogger logger)
        {
            this.restClient = new RestClient(baseUrl);
            this.logger = logger;
        }

        public async Task PostAsync<TEntity>(string query, TEntity entity, Dictionary<string, string>? headers = null) where TEntity : class, new()
        {
            try
            {
                var request = new RestRequest(query, Method.Post);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(entity);
                AddHeaders(request, headers);

                var result = await restClient.ExecuteAsync(request);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public async Task<TEntity> GetAsync<TEntity, TError>(string query, Dictionary<string, string>? headers = null) where TEntity : class, new() where TError : class, IError, new()
        {
            var request = new RestRequest(query, Method.Get);
            AddHeaders(request, headers);

            // may return TResponse or TError
            var result = await restClient.GetAsync(request);

            if (IsErrorOccurred<TError>(result, out TError errorModel))
            {
                logger.LogInformation(errorModel?.Error);
                return null;
            }

            return JsonConvert.DeserializeObject<TEntity>(result.Content);
        }

        public async Task DeleteAsync(string query, Dictionary<string, string>? headers = null)
        {
            try
            {
                var request = new RestRequest(query, Method.Delete);
                AddHeaders(request, headers);

                await restClient.ExecuteAsync(request);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        private void AddHeaders(RestRequest request, Dictionary<string, string>? headers)
        {
            if (headers == null)
            {
                return;
            }

            foreach (var key in headers.Keys)
            {
                request.AddHeader(key, headers[key]);
            }
        }

        private bool IsErrorOccurred<TError>(RestResponse? result, out TError? errorModel) where TError : class, IError, new()
        {
            try
            {
                errorModel = JsonConvert.DeserializeObject<TError>(result.Content);
                if (!string.IsNullOrEmpty(errorModel?.Error))
                {
                    logger.LogInformation(errorModel.Error);
                    return true;
                }

                return false;
            }
            catch
            {
                errorModel = null;
                return false;
            }
        }
    }
}
