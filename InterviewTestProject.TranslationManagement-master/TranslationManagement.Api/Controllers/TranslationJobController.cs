using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OneOf.Types;
using TranslationManagement.Api.Controlers;
using TranslationManagement.Application.Database;
using TranslationManagement.Application.Models;
using TranslationManagement.Application.Services;

using TranslationManagement.Api.Mappings;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    public class TranslationJobController : ControllerBase
    {
        private AppDbContext _context;
        private readonly ILogger<TranslatorManagementController> _logger;

        private readonly ITranslationJobService _translationJobService;

        public TranslationJobController(
            IServiceScopeFactory scopeFactory, 
            ILogger<TranslatorManagementController> logger, 
            ITranslationJobService translationJobService)
        {
            _context = scopeFactory.CreateScope().ServiceProvider.GetService<AppDbContext>();
            _logger = logger;
            _translationJobService = translationJobService;
        }

        [HttpGet(ApiEndpoints.Jobs.GetAll)]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = await _translationJobService.GetAllJobsAsync();

            return Ok(jobs);
        }

        [HttpPost(ApiEndpoints.Jobs.Create)]
        public async Task<IActionResult> CreateJob(TranslationJob job)
        {
            var result = await _translationJobService.Create(job);

            return result.Match<IActionResult>(
                _ => Ok(),
                failed => BadRequest(failed.MapToResponse())
                );
        }

        [HttpPost(ApiEndpoints.Jobs.CreateWithFile)]
        public async Task<IActionResult> CreateJobWithFile(IFormFile file, string customer)
        {
            var result = await _translationJobService.CreateWithFile(file, customer);

            return result.Match<IActionResult>(
                _ => Ok(),
                failed => BadRequest(failed.MapToResponse())
                );
        }

        [HttpPost(ApiEndpoints.Jobs.UpdateStatus)]
        public async Task<IActionResult> UpdateJobStatus(int jobId, int translatorId, string newStatus = "")
        {
            _logger.LogInformation("Job status update request received: {jobStatus} for job {jobId} by translator {translatorId}", newStatus, jobId, translatorId);

            var result = await _translationJobService.UpdateJobStatus(jobId, newStatus);

            return result.Match<IActionResult>(
                _ => Ok(),
                _ => NotFound(),
                failed => BadRequest(failed.MapToResponse())
                );
        }
    }
}
