using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Mappings;
using TranslationManagement.Application.BackgroundWorkers;
using TranslationManagement.Application.Database;
using TranslationManagement.Application.Models;
using TranslationManagement.Application.Services;

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
        public async Task<IActionResult> GetTranslators()
        {
            return Ok(await _translatorModelService.GetAllTranslators());
        }

        [HttpGet(ApiEndpoints.Translators.GetTranslatorsByName)]
        public async Task<IActionResult> GetTranslatorsByName(string name)
        {
            var translators = await _translatorModelService.GetTranslatorsByName(name);

            return Ok(translators);
        }

        [HttpPost(ApiEndpoints.Translators.Create)]
        public async Task<IActionResult> CreateTranslator(TranslatorModel translator)
        {
            var result = await _translatorModelService.Create(translator);

            return result.Match<IActionResult>(
                _ => Ok(),
                failed => BadRequest(failed.MapToResponse())
                );
        }

        [HttpPost(ApiEndpoints.Translators.UpdateStatus)]
        public async Task<IActionResult> UpdateTranslatorStatus(int translatorId, string newStatus = "")
        {
            var result = await _translatorModelService.UpdateStatus(translatorId, newStatus);

            return result.Match<IActionResult>(
                _ => Ok(),
                _ => NotFound(),
                failed => BadRequest(failed.MapToResponse())
            );
        }
    }
}
