using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TranslationManagement.Application.Models;
using TranslationManagement.Contracts.Requests;
using TranslationManagement.Contracts.Responses;

namespace TranslationManagement.Api.Mappings;

public static class ContractMapping
{
    public static TranslationJob MapToJob(this CreateTranslationJobRequest request)
    {
        return new TranslationJob
        {
            CustomerName = request.CustomerName,
            OriginalContent = request.OriginalContent,
            Status = nameof(JobStatuses.New),
            TranslatedContent = request.TranslatedContent,
        };
    }

    public static TranslationJobResponse MapToResponse(this TranslationJob job)
    {
        return new TranslationJobResponse
        {
            Id = job.Id,
            CustomerName = job.CustomerName,
            OriginalContent = job.OriginalContent,
            Status = job.Status,
            TranslatedContent = job.TranslatedContent,
            Price = job.Price,
        };
    }

    public static TranslationJobsResponse MapToResponse(this IEnumerable<TranslationJob> jobs)
    {
        return new TranslationJobsResponse
        {
            Items = jobs.Select(MapToResponse)
        };
    }

    public static TranslatorModel MapToTranslator(this CreateTranslatorModelRequest request)
    {
        return new TranslatorModel
        {
            CreditCardNumber = request.CreditCardNumber,
            HourlyRate = request.HourlyRate,
            Name = request.Name,
            Status = request.Status,
        };
    }


    public static TranslatorModelResponse MapToResponse(this TranslatorModel translator)
    {
        return new TranslatorModelResponse
        {
            Id = translator.Id,
            CreditCardNumber = translator.CreditCardNumber,
            HourlyRate = translator.HourlyRate,
            Name = translator.Name,
            Status = translator.Status,
        };
    }

    public static TranslatorModelsResponse MapToResponse(this IEnumerable<TranslatorModel> translators)
    {
        return new TranslatorModelsResponse
        {
            Items = translators.Select(MapToResponse)
        };
    }
}
