using Amazon.Runtime.Internal.Util;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Memory;
using SearchLog.Api.Configurations;
using SearchLog.Api.Models;
using SearchLog.Business.Abstract;
using SearchLog.DataAccess.Abstract;
using SearchLog.Entities.Concrete;
using ValueBlue.Core.Entities.Concrete;

namespace SearchLog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchLogController : ControllerBase
    {
        private readonly ISearchLogService searchLogService;
        private readonly ILogger<SearchLogController> logger;
        private readonly IMemoryCache cache;
        private readonly IMapper mapper;

        public SearchLogController(ISearchLogService searchLogService, ILogger<SearchLogController> logger, IMemoryCache cache, IMapper mapper)
        {
            this.searchLogService = searchLogService;
            this.logger = logger;
            this.cache = cache;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAll([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
        {
            try
            {
                Pagination pagination = new Pagination { Page = page, PageSize = pageSize };
                var cacheKey = $"{page.ToString()}_{pageSize.ToString()}";

                if (cache.TryGetValue(cacheKey, out IEnumerable<SearchLogDocument>? data))
                {
                    return Ok(data);
                }

                var documents = await searchLogService.GetAllRecordsAsync(pagination);
                if (documents == null || !documents.Any())
                {
                    return NoContent();
                }

                cache.Set(cacheKey, documents, CacheConfiguration.GetCacheOptions());
                return Ok(documents);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            try
            {
                if (cache.TryGetValue(id, out SearchLogDocument? data))
                {
                    return Ok(data);
                }

                var document = await searchLogService.GetSearchLogByIdAsync(id);
                if (document == null)
                {
                    return NoContent();
                }

                cache.Set(id, document, CacheConfiguration.GetCacheOptions());

                return Ok(document);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetByDatePeriod")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetByDatePeriod([FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] int? page = null, [FromQuery] int? pageSize = null)
        {
            try
            {
                Pagination pagination = new Pagination { Page = page, PageSize = pageSize };

                string cacheKey = $"{startDate.ToString()}_{endDate.ToString()}_{page}_{pageSize}";

                if (cache.TryGetValue(cacheKey, out IEnumerable<SearchLogDocument>? data))
                {
                    return Ok(data);
                }

                var documents = await searchLogService.GetSearchLogByDatePeriodAsync(startDate, endDate, pagination);
                if (documents == null || !documents.Any())
                {
                    return NoContent();
                }

                cache.Set(cacheKey, documents, CacheConfiguration.GetCacheOptions());

                return Ok(documents);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDailyUsageReport/{date}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetDailyUsageReport([FromRoute] string date)
        {
            try
            {
                if (cache.TryGetValue(date, out DailyUsageReport? data))
                {
                    return Ok(data);
                }

                var report = await searchLogService.GetDailyUsageReportAsync(date);
                if (report == null)
                {
                    return Ok(new DailyUsageReport { Count = 0, Date = date});
                }

                cache.Set(date, report, CacheConfiguration.GetCacheOptions());

                return Ok(report);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SearchLogRequestModel model)
        {
            try
            {
                var document = mapper.Map<SearchLogDocument>(model);

                await searchLogService.AddSearchLogAsync(document);
                return Created();
            }
            catch (ValidationException ex)
            {
                logger.LogError(ex.Message, ex);
                return StatusCode(422, ex.Message);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                var entity = await searchLogService.GetSearchLogByIdAsync(id);
                if (entity == null)
                {
                    return NoContent();
                }

                await searchLogService.DeleteSearchLogAsync(id);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
