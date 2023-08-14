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
using TranslationManagement.Contracts.Requests;
using TranslationManagement.Contracts.Responses;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    public class TranslationJobController : ControllerBase
    {
        private readonly ILogger<TranslatorManagementController> _logger;

        private readonly ITranslationJobService _translationJobService;

        public TranslationJobController(
            ILogger<TranslatorManagementController> logger, 
            ITranslationJobService translationJobService)
        {
            _logger = logger;
            _translationJobService = translationJobService;
        }

        [HttpGet(ApiEndpoints.Jobs.GetAll)]
        [ProducesResponseType(typeof(TranslationJobsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = await _translationJobService.GetAllJobsAsync();

            return Ok(jobs.MapToResponse());
        }

        [HttpPost(ApiEndpoints.Jobs.Create)]
        [ProducesResponseType(typeof(TranslationJobResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateJob([FromBody]CreateTranslationJobRequest request)
        {
            var job = request.MapToJob();

            var result = await _translationJobService.Create(job);

            return result.Match<IActionResult>(
                _ => Created("createdUrlTODO", job.MapToResponse()),
                failed => BadRequest(failed.MapToResponse())
                );
        }

        [HttpPost(ApiEndpoints.Jobs.CreateWithFile)]
        [ProducesResponseType(typeof(TranslationJobResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateJobWithFile(IFormFile file, string customer)
        {
            var result = await _translationJobService.CreateWithFile(file, customer);
            return result.Match<IActionResult>(
                _ => Created("createdUrlTODO", result.AsT0),
                failed => BadRequest(failed.MapToResponse())
                );
        }

        [HttpPost(ApiEndpoints.Jobs.UpdateStatus)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateJobStatus([FromBody] UpdateTranslationJobStatusRequest request)
        {
            _logger.LogInformation(
                "Job status update request received: {jobStatus} for job {jobId} by translator {translatorId}",
                request.NewStatus, request.JobId, request.TranslatorId);

            var result = await _translationJobService.UpdateJobStatus(request.JobId, request.NewStatus);

            return result.Match<IActionResult>(
                _ => Ok(),
                _ => NotFound(),
                failed => BadRequest(failed.MapToResponse())
                );
        }
    }
}
