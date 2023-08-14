using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Mappings;
using TranslationManagement.Application.BackgroundWorkers;
using TranslationManagement.Application.Database;
using TranslationManagement.Application.Models;
using TranslationManagement.Application.Services;
using TranslationManagement.Contracts.Requests;
using TranslationManagement.Contracts.Responses;

namespace TranslationManagement.Api.Controlers
{
    [ApiController]
    public class TranslatorManagementController : ControllerBase
    {
        private readonly ILogger<TranslatorManagementController> _logger;
        private readonly ITranslatorModelService _translatorModelService;

        public TranslatorManagementController(
            ILogger<TranslatorManagementController> logger,
            ITranslatorModelService translatorModelService
            )
        {
            _logger = logger;
            _translatorModelService = translatorModelService;
        }

        [HttpGet(ApiEndpoints.Translators.GetAll)]
        [ProducesResponseType(typeof(TranslatorModelsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTranslators()
        {
            var translators = await _translatorModelService.GetAllTranslators();

            return Ok(translators.MapToResponse());
        }

        [HttpGet(ApiEndpoints.Translators.GetTranslatorsByName)]
        [ProducesResponseType(typeof(TranslatorModelsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTranslatorsByName(string name)
        {
            var translators = await _translatorModelService.GetTranslatorsByName(name);

            return Ok(translators.MapToResponse());
        }

        [HttpPost(ApiEndpoints.Translators.Create)]
        [ProducesResponseType(typeof(TranslatorModelResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTranslator(CreateTranslatorModelRequest request)
        {
            var translator = request.MapToTranslator();

            var result = await _translatorModelService.Create(translator);
            return result.Match<IActionResult>(
                _ => Created("createdUrlTODO", translator.MapToResponse()),
                failed => BadRequest(failed.MapToResponse())
                );
        }

        [HttpPost(ApiEndpoints.Translators.UpdateStatus)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTranslatorStatus([FromBody] UpdateTranslatorStatusRequest request)
        {
            var result = await _translatorModelService.UpdateStatus(
                request.TranslatorId, request.NewStatus);

            return result.Match<IActionResult>(
                _ => Ok(),
                _ => NotFound(),
                failed => BadRequest(failed.MapToResponse())
            );
        }
    }
}
