using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Core.Entities.Abstract;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ValueBlue.Core.REST
{
    public interface IHttpRepository
    {
        Task<TEntity> GetAsync<TEntity, TError>(string query, Dictionary<string, string>? headers = null) where TEntity : class, new() where TError : class, IError, new();

        Task PostAsync<TEntity>(string query, TEntity entity, Dictionary<string, string>? headers = null) where TEntity : class, new();

        Task DeleteAsync(string query, Dictionary<string, string>? headers = null);
    }
}
