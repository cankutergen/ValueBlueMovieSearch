using Admin.Api.Configurations;
using Admin.Business.Abstract;
using Admin.Business.Concrete;
using Admin.Entities.Concrete;
using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Memory;
using ValueBlue.Core.Entities.Concrete;

namespace Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        private readonly ILogger<AdminController> logger;
        private readonly IMemoryCache cache;

        public AdminController(IAdminService adminService, ILogger<AdminController> logger, IMemoryCache cache)
        {
            this.adminService = adminService;
            this.logger = logger;
            this.cache = cache;
        }

        //todo: add unit tests
        //todo: add gateway?
        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAll([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
        {
            try
            {
                Pagination pagination = new Pagination { Page = page, PageSize = pageSize };
                var cacheKey = $"{page.ToString()}_{pageSize.ToString()}";

                if (cache.TryGetValue(cacheKey, out List<SearchLogModel>? data))
                {
                    return Ok(data);
                }

                var documents = await adminService.GetAllRecordsAsync(authorizationHeader: GetAuthorizationHeader(), pagination: pagination);
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
                if (cache.TryGetValue(id, out SearchLogModel? data))
                {
                    return Ok(data);
                }

                var document = await adminService.GetSearchLogByIdAsync(id, GetAuthorizationHeader());
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

                if (cache.TryGetValue(cacheKey, out List<SearchLogModel>? data))
                {
                    return Ok(data);
                }

                var documents = await adminService.GetSearchLogByDatePeriodAsync(startDate, endDate, authorizationHeader: GetAuthorizationHeader(), pagination: pagination);
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

                var report = await adminService.GetDailyUsageReportAsync(date, GetAuthorizationHeader());
                if (report == null)
                {
                    return NoContent();
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

        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                await adminService.DeleteSearchLogAsync(id, GetAuthorizationHeader());

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

        private Dictionary<string, string>? GetAuthorizationHeader()
        {
            string authorizationHeader = this.HttpContext?.Request?.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return null;
            }

            return new Dictionary<string, string> { { "Authorization", authorizationHeader } };
        }
    }
}
